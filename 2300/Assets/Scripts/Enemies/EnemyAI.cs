using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [HideInInspector]
    public GameObject player; // store reference to player so you don't have to keep looking up the player.

    private Transform target; // target transform so the enemy can look at objects other than the player.

    public float speed; // how fast does the enemy move.

    private Rigidbody2D rb;

    public bool selfDestruct = true;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) Debug.LogError($"EnemyAI.cs: {name} couldn't find player GameObject.\n\nCheck that the player gameobject has the tag \"player\" and is in this scene.");
        target = player?.transform;
        StartCoroutine(checkDistances());
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private float kill = 0;
    // Update is called once per frame
    void Update()
    {
        if (selfDestruct)
        {
            if (kill > 10)
            {
                kill = 0;
                gameObject.SetActive(false);
            }
            kill += Time.deltaTime;
        }
        
        rb.velocity = (target.position - transform.position).normalized * speed;
    }

    private void updateTarget(Transform p)
    {
        if (p == target) return; // if we are already chasing the portal exit early.

        var a = Vector3.Distance(player.transform.position, transform.position); // distance to player
        var b = Vector3.Distance(p.position, transform.position); // distance to portal

        if (a > b)
            target = p;
    }

    private IEnumerator checkDistances()
    {
        // you don't want every enemy to check distance at the same time that will cause lag spikes
        var wait = new WaitForSeconds(Random.Range(1, 5));

        while (true)
        {
            yield return wait;

            GameObject[] results = GameObject.FindGameObjectsWithTag("Portal"); // find all portals in scene
            Transform closePortal = null; // closest portal
            Transform farPortal = null; // farthest portal
            float d1 = Mathf.Infinity;
            float d2 = -1;
            foreach (var p in results)
            {
                float b = Vector3.Distance(p.transform.position, transform.position); // distance to given portal
                if (b < d1) // find closest portal
                {
                    closePortal = p.transform;
                    d1 = b;
                }
                if (b > d2) // find farthest portal
                {
                    farPortal = p.transform;
                    d2 = b;
                }
            }

            float a = Vector3.Distance(transform.position, player.transform.position); // distance to player
            float ap = Vector3.Distance(player.transform.position, farPortal.position); // distance player to farPortal

            // if player is farther away than closest portal
            // and the distance to closest portal + distance from farthest portal to player is less than distance to player then use the closest portal.
            if (a > d1 && d1 + ap < a)
                updateTarget(closePortal);
            else // otherwise you should just look to chase the player.
                target = player.transform;
        }
    }
}
