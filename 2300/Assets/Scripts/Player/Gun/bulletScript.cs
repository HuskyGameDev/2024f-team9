using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rb;
    public float force;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        rb.velocity = new Vector2(direction.x , direction.y).normalized * force;
        StartCoroutine(DecayAfter(force));
    }

    private IEnumerator DecayAfter(float sec)
    {
        yield return new WaitForSeconds(sec);

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
