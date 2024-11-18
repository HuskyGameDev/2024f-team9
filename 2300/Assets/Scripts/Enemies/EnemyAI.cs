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
    private Animator anim;

    public bool debugSelfDestruct = true;
    public EnemyHealth enemyHealth;

    private enum State { idle, runl, runr, runu, rund };
    private State state = State.idle;


    private void OnEnable()
    {
        state = State.idle;
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) Debug.LogError($"EnemyAI.cs: {name} couldn't find player GameObject.\n\nCheck that the player gameobject has the tag \"player\" and is in this scene.");
        target = player?.transform;
        StartCoroutine(checkDistances());
    }

    private void OnDisable()
    {
        StopCoroutine(checkDistances()); // cleanup during disable so nothing is being run in the background when this object is not in "play".
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private float kill = 0;
    // Update is called once per frame
    void Update()
    {
        if (enemyHealth.health <= 0)
        {
            gameObject.SetActive(false);
        }

        anim.SetInteger("state", (int)state);
        angleState((target.position - transform.position).normalized);

        rb.velocity = (target.position - transform.position).normalized * speed;
    }

    private void angleState(Vector2 lookDir)
    {
        var up = Vector2.Dot(lookDir, Vector2.up); // 1 if lookDir is looking up. 0 if looking to the right/left. -1 if looking down.
        var right = Vector2.Dot(lookDir, Vector2.right); // 1 if looking right. 0 if looking up or down. -1 if looking left.

        if (Mathf.Abs(up) >= Mathf.Abs(right))
        {
            if (up > 0)
                state = State.runu;
            else if (up < 0)
                state = State.rund;
        }
        else if (Mathf.Abs(right) > Mathf.Abs(up))
        {
            if (right > 0)
                state = State.runr;
            else if (right < 0)
                state = State.runl;
        }
        else
            state = State.idle;
    }

    private IEnumerator checkDistances()
    {
        // you don't want every enemy to check distance at the same time that will cause lag spikes
        var wait = new WaitForSeconds(Random.Range(1, 3));


        GameObject[] results = GameObject.FindGameObjectsWithTag("Portal"); // find all portals in scene

        while (true && results.Length > 0)
        {
            yield return wait;

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
                target = closePortal;
            else // otherwise you should just look to chase the player.
                target = player.transform;
        }
    }
}
