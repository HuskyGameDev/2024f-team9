using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    Vector2 movementInput;
    Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }



    private void FixedUpdate(){
        if (movementInput != Vector2.zero){
            bool success = TryMove(movementInput);

            if(!success) {
                success = TryMove (new Vector2(movementInput.x, 0));

                if(!success) {
                    success = TryMove(new Vector2(0, movementInput.y));
                }
            }
           
        }
    }

    private bool TryMove (Vector2 direction) {
        if (direction != Vector2.zero){
            int count = rb.Cast(
                    movementInput,
                    movementFilter,
                    castCollisions,
                    moveSpeed * Time.fixedDeltaTime + collisionOffset);


            if(count == 0){
                        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                        return true;
            } else {
                    return false;
            }
        } else {
            return false;
        }
    }

    void OnMove(InputValue movementValue) {
        movementInput = movementValue.Get<Vector2>();
    }
}
