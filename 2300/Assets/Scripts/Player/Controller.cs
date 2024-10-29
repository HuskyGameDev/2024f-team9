using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public float speed = 5;
    private Animator anim;
    private Rigidbody2D rb;
    public GameObject gun;
    public GameObject crosshair;
    public Camera cam;
    private float hdirection;
    private float vdirection;
    Vector2 mousePos;
    Vector2 xRef = new Vector2(.5f, .5f);
    Vector2 yRef = new Vector2(.5f, -.5f);

    private enum State {idle, runl, runr, runu, rund};
    private State state = State.idle;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        hdirection = Input.GetAxisRaw("Horizontal");
        vdirection = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        Vector2 deltaX = dotProdX(xRef, mousePos);
        Vector2 deltaY = dotProdY(yRef, mousePos);
        
        faceState(deltaX, deltaY);

        anim.SetInteger("state", (int)state);
    }

    private void FixedUpdate()
    {
        MovePlayer(hdirection, vdirection);
        MoveCursor(mousePos);
    }

    void MovePlayer(float h, float v) 
    {
         Vector2 targetVelocity = new Vector2(h * speed, v * speed);
         rb.velocity = new Vector2(targetVelocity.x, targetVelocity.y); 

    }

    void MoveCursor(Vector2 mousePos)
    {
        crosshair.transform.position = mousePos;
    }

   

    private void faceState(Vector2 deltaX, Vector2 deltaY)
    {
       if (deltaX.x > 0 && deltaY.y > 0)
        {
            state = State.runr;
        }
        if (deltaX.x < 0 && deltaY.y > 0)
        {
            state = State.rund;
        }
        if (deltaX.x < 0 && deltaY.y < 0)
        {
            state = State.runl;
        }
        if (deltaX.x > 0 && deltaY.y < 0)
        {
            state = State.runu;
        }
        
    }

    private Vector2 dotProdX(Vector2 xRef, Vector2 mousePos) {
        return new Vector2((xRef.x * mousePos.x),(xRef.y * mousePos.y));
    }

    private Vector2 dotProdY(Vector2 yRef, Vector2 mousePos) {
        return new Vector2((yRef.x * mousePos.x),(yRef.y * mousePos.y));
    }

    
}
