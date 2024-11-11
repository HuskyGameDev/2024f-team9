using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Enemy
    {
        [Tooltip("How likely is this enemy to spawn?"), Range(0, 1)]
        public float weight = 1;
        [Tooltip("Enemy Prefab GameObject")]
        public GameObject prefab;
    }

    public List<Enemy> enemies;
    private List<GameObject> enemyObjects; // object pooling for enemies (we don't need to always spawn more if an enemy is already dead we can just reuse them).
    private GameObject highestWeightEnemyPrefab; // fallback enemy to spawn when weights don't add up to < 1

    public int spawnAmount = 5; // starting amount of enemies spawned
    [Tooltip("By what percent do you want the amount of enemies to increase each wave?"),Range(0f,2f)]
    public float spawnIncrease = 0.1f;
    private float _spawnAmount; // actual amount spawned

    public float timeToSpawn;
    public float spawnRadius = 10;
    [Tooltip("Minimum distance away from player")]
    public float spawnLeniency = 2;
    private float spawnCounter;

    private Transform player;

    private void OnValidate()
    {
        float totalWeight = 0;
        foreach (Enemy enemy in enemies)
        {
            if (enemy.prefab == null) Debug.LogWarning($"EnemySpawner.cs: Enemy prefab undefined at index ({enemies.IndexOf(enemy)})");
            totalWeight += enemy.weight;
        }
        if (totalWeight > 1.0) Debug.LogWarning($"EnemySpawner.cs: Total weight value ({totalWeight}) exceeds 1.0 some enemies will not spawn");

        if (spawnLeniency > spawnRadius) Debug.LogWarning($"EnemySpawner.cs: Spawn Leniency({spawnLeniency}) > spawn Radius({spawnRadius}). Enemies will always spawn {spawnLeniency} units away.");
    }

    // Start is called before the first frame update
    void Start()
    {
        _spawnAmount = spawnAmount;
        float weight = 0;
        foreach (Enemy enemy in enemies) // set the fallback enemy before the game begins.
            if (enemy.weight > weight) // the ? basically prevents null errors by checking "is this object null?" if it is then it just returns null otherwise it returns the value you're looking for.
                highestWeightEnemyPrefab = enemy.prefab;

        spawnCounter = timeToSpawn;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null) Debug.LogError($"EnemySpawner.cs: {name} couldn't find player GameObject.\n\nCheck that the player GameObject has the tag \"Player\" and is in this scene.");

        enemyObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        spawnCounter -= Time.deltaTime;
        if (spawnCounter <= 0)
        {
            spawnCounter = timeToSpawn;

            GameObject[] results = GameObject.FindGameObjectsWithTag("Portal"); // find all portals in scene
            Transform p = null; // closest portal
            float d = Mathf.Infinity;
            foreach (var portal in results)
            {
                float dp = Vector3.Distance(portal.transform.position, player.position); // distance to given portal
                if (dp < d) // find closest portal
                {
                    p = portal.transform;
                    d = dp;
                }
            }

            //Debug.Log($"Spawning {Mathf.FloorToInt(_spawnAmount)} Enemies");
            for (int i = 0; i < Mathf.FloorToInt(_spawnAmount); i++)
            {
                var enemyPrefab = enemyToSpawn();
                var a = (Vector3)Random.insideUnitCircle * spawnRadius; // full spawn radius
                var b = (a).normalized * spawnLeniency; // leniency on spawn radius preventing enemies from spawning ontop of the spawner (player)
                var c = Vector3.Distance(a + player.position, player.position) < Vector3.Distance(b + player.position, player.position) ? b : a;
                
                // check if enemy would spawn on other side of portal.
                var pxz = new Vector3(p.position.x, player.position.y, p.position.z);
                var pdir = (pxz - player.position);
                Debug.DrawLine(player.position, player.position+ pdir.normalized, Color.green, timeToSpawn);
                if (Vector3.Dot(pdir.normalized, c.normalized) > 0f && Vector3.Distance(pxz, player.position) <= Vector3.Distance(c + player.position, player.position)) // if a enemy would spawn in the same direction as a portal and be outside the portal spawn them flipped onto the other side.
                    c = -c;
                
                Debug.DrawLine(player.position, player.position + c.normalized, Color.blue, 5);
                var inactiveEnemy = enemyObjects.Find(i => i.activeInHierarchy == false && i.name.Equals($"{enemyPrefab.name}(Clone)")); // check if there are any enemy objects that are already inactive.
                if (inactiveEnemy == null) // if there are no inactive gameobjecst then create a new gameobject
                {
                    var enemy = Instantiate(enemyPrefab, player.position + c, player.rotation);
                    enemyObjects.Add(enemy);
                }
                else // if there is a inactive gameobject then reenable it and set its position and rotation!
                {
                    inactiveEnemy.SetActive(true);
                    inactiveEnemy.transform.position = player.position + c;
                    inactiveEnemy.transform.rotation = player.rotation;
                }
            }

            _spawnAmount += _spawnAmount*spawnIncrease;
        }
    }

    private GameObject enemyToSpawn()
    {
        float rand = Random.value;
        float chance = 0;
        foreach (Enemy enemy in enemies) // check each enemy in the enemies array.
        {
            if (rand < chance + enemy.weight) // if the value is less than the current chance + enemy weight then return this enemy.
                return enemy.prefab;
            chance += enemy.weight; // every attempt increase the chance for the mob to spawn by the weight. so that you have a porportional weighting spawn.
        }

        return highestWeightEnemyPrefab; // if no enemy passes the enemy weight check then by default spawn the enemy with the highest weight value since it's the "most" common enemy type.
    }
}
