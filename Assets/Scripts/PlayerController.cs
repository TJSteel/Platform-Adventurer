using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public Text countText;
    public CharacterController2D controller;
    public float runSpeed = 40f;
    public Animator animator;
    public Rigidbody2D rb2d;

    private int itemCount;
    private float moveHorizontal = 0f;
    private bool jump = false;
    private bool crouch = false;
    private bool falling = false;
    private bool canDoubleJump = true;
 
    // Start is called before the first frame update
    void Start() {
        itemCount = 0;
    }

    // Update is called once per frame
    void Update() {
        countText.text = "Count: " + itemCount;
        moveHorizontal = Input.GetAxisRaw("Horizontal") * runSpeed;
        falling = rb2d.velocity.y < -0.01;

        animator.SetFloat("Speed", Mathf.Abs(moveHorizontal));
        animator.SetBool("IsFalling", falling);
        if(falling) animator.SetBool("IsJumping", false);

        if (Input.GetButtonDown("Jump")) {
            if (Mathf.Abs(rb2d.velocity.y) > 0.1){
                if (canDoubleJump){
                    jump = true;
                    canDoubleJump = false;
                    animator.SetBool("DoubleJump", true);
                    animator.SetBool("IsJumping", true);
                }
            } else {
                jump = true;
                animator.SetBool("IsJumping", true);
            }
        }
        if (Input.GetButtonDown("Crouch")) {
            crouch = true;
        } else if (Input.GetButtonUp("Crouch")) {
            crouch = false;
        }
    }

    // Fixed update is called just before calculating any physics
    private void FixedUpdate() {
        controller.Move(moveHorizontal * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    //OnTriggerEnter2D is called whenever this object overlaps with a trigger collider.
    void OnTriggerEnter2D(Collider2D other)
    {
        //Check the provided Collider2D parameter other to see if it is tagged "PickUp", if it is...
        if (other.gameObject.CompareTag("Pickup")) {
            other.gameObject.SetActive(false);
            itemCount++;
            
        }
    }

    public void OnLanded(){
        canDoubleJump = true;
        animator.SetBool("DoubleJump", false);    
    }
}
