using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D),typeof(BoxCollider2D))]
public class Controller : MonoBehaviour
{
    private Tilemap radiationLayer;

    private TileBase radiation1;
    private TileBase radiation2;
    private TileBase radiation3;
    private TileBase radiation4;
    private TileBase radiation5;

    public float speed = 5;
    private Animator anim;
    private Rigidbody2D rb;
    public GameObject playerGFX;
    public GameObject gun;
    public GameObject crosshair;
    public Camera cam;
    private float hdirection;
    private float vdirection;
    private float angle;
    public Vector2 xRef;
    public Vector2 yRef;
    private Vector3 mousePos;
    private Vector3 crossHair;

    private enum State {idle, runl, runr, runu, rund};
    private State state = State.idle;

    public HealthManager healthManager;
    private float damageTimer = 0.0f;
    private float damageInterval = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        radiationLayer = GameObject.FindGameObjectWithTag("Radiation").GetComponent<Tilemap>();

        var map = radiationLayer.GetComponentInParent<GenerateLevel>();
        radiation1 = map.radiationTiles[0];
        radiation2 = map.radiationTiles[1];
        radiation3 = map.radiationTiles[2];
        radiation4 = map.radiationTiles[3];
        radiation5 = map.radiationTiles[4];

        rb = GetComponent<Rigidbody2D>();
        anim = playerGFX.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 crossHair = cam.ScreenToWorldPoint(Input.mousePosition);
        MoveCursor(crossHair);
        float normalizedX = (mousePos.x / Screen.width) * 2 - 1; // Normalize to -1 to 1
        float normalizedY = (mousePos.y / Screen.height) * 2 - 1; // Normalize to -1 to 1
        // Calculate angle to face
        angle = Mathf.Atan2(normalizedY, normalizedX) * Mathf.Rad2Deg;
        if (angle < 360) {
            angle += 360;
        }
        anim.SetInteger("state", (int)state);
        AngleState(angle);

        hdirection = Input.GetAxisRaw("Horizontal");
        vdirection = Input.GetAxisRaw("Vertical");
        
    }

    private void FixedUpdate()
    {
        MovePlayer(hdirection, vdirection);

        Vector3 position = transform.position;
        Vector3Int positionInt = Vector3Int.FloorToInt(position);
        var tile = radiationLayer.GetTile(positionInt);

        float radiationDamage = 0.0f;

        //set radiation damage based on radiation tile
        if (tile == radiation1)
        {
            radiationDamage = 1.0f;
        }
        else if (tile == radiation2)
        {
            radiationDamage = 2.0f;
        }
        else if (tile == radiation3)
        {
            radiationDamage = 3.0f;
        }
        else if (tile == radiation4)
        {
            radiationDamage = 4.0f;
        }
        else if (tile == radiation5)
        {
            radiationDamage = 5.0f;
        }

        //apply damage every second that the player is on a radiation tile
        damageTimer += Time.deltaTime;
        if (damageTimer >= damageInterval)
        {
            healthManager.TakeDamage(radiationDamage);
            damageTimer = 0.0f;
        }
    }

    void MovePlayer(float h, float v) 
    {
         Vector2 targetVelocity = new Vector2(h * speed, v * speed);
         rb.velocity = new Vector2(targetVelocity.x, targetVelocity.y); 
    }

    void MoveCursor(Vector3 mousePos)
    {
        mousePos.z = 0;
        crosshair.transform.position = mousePos;
    }

    private void AngleState (float angle) {
        if (angle < 490 && angle > 400 ) {
            state = State.runu;
        }
        if ((angle > 490 || angle > 180) && angle < 225) {
            state = State.runl;
        }
        if (angle > 225 && angle < 315) {
            state = State.rund;
        }
        if (angle > 315 && angle < 400) {
            state = State.runr;
        }
    }

    public void PlayerDied()
    {
        LevelManager.instance.GameOver();
        gameObject.SetActive(false);
    }
}
