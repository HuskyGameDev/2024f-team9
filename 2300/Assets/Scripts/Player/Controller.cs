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
      

        anim.SetInteger("state", (int)state);
    }

    private void FixedUpdate()
    {
        MovePlayer(hdirection, vdirection);
        MoveCursor(mousePos);

        

        Vector2 lookDir = mousePos - rb.position;
        Vector3 eulerAngles = rb.transform.rotation.eulerAngles;
        float angX = eulerAngles.x;
        float angY = eulerAngles.y;
        float angle = 0;
        Debug.Log(angX);
        Debug.Log(angY);
        //float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        //rb.rotation = angle;
        AngleState(angle);

        
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

   

    private void AngleState(float angle)
    {
        if (angle <= 145 && angle >=35) {
            state = State.runu;
        }
        if (angle <=35 && angle >= -35) {
            state = State.runl;
        }
        if (angle >= -145 && angle <= -35) {
            state = State.rund;
        } 
    }
}
