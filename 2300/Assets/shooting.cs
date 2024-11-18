using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class shooting : MonoBehaviour
{
    private Camera mainCam;
    public AudioSource shootSound;
    private Vector3 mousePos;
    public GameObject bullet;
    public Transform bulletTransform;
    public bool canFire;
    private float timer;
    public float timeBetweenFiring;
    private SpriteRenderer rend;
    // Start is called before the first frame update
    void Start()
    {
       mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
       rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos - transform.position;
        //float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0, 0, rotZ);
        if(!canFire){
            timer += Time.deltaTime;
            if(timer > timeBetweenFiring){
                canFire = true;
                timer = 0;
            }
        }

        if( Input.GetMouseButton(0) && canFire){
            canFire = false;
            GameObject newBullet = Instantiate(bullet, bulletTransform.position, Quaternion.identity);
            shootSound.Play();
            int spawn = rend.sortingOrder;
            SpriteRenderer bulletRenderer = newBullet.GetComponent<SpriteRenderer>();
            bulletRenderer.sortingOrder = spawn + 1;
        }
    }
}
