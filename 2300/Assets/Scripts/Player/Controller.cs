using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(BoxCollider2D))]
public class Controller : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
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

}
