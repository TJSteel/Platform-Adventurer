using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public Text countText;
    public float runSpeed;
    public float jumpForce;
    public Animator animator;
    public Rigidbody2D rb2d;
    public float scaleModifier; // this is to convert the players scale into meters
    public float moveAcceleration;

    private int itemCount;
    private float moveHorizontal = 0f;
    private bool jump = false;
    private bool crouch = false;
    private bool falling = false;
    private bool canDoubleJump = true;
    private bool facingRight = true;
 
    // Start is called before the first frame update
    void Start() {
        itemCount = 0;
    }

    // Update is called once per frame
    void Update() {

        // calculate animation effects

        countText.text = "Count: " + itemCount;
        moveHorizontal = Input.GetAxisRaw("Horizontal") * runSpeed;

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

        animator.SetFloat("Speed", Mathf.Abs(moveHorizontal));
        animator.SetBool("IsFalling", falling);
        OrientPlayer();
    }

    // Fixed update is called just before calculating any physics
    private void FixedUpdate() {

        Vector2 velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y);
        falling = velocity.y < -0.01;
        velocity.x += moveHorizontal * moveAcceleration * Time.fixedDeltaTime;
        if (velocity.x > runSpeed * scaleModifier){
            velocity.x = runSpeed * scaleModifier;
        } else if (velocity.x < -runSpeed * scaleModifier){
            velocity.x = -runSpeed * scaleModifier;
        }
        if (jump) {
            velocity.y += jumpForce * scaleModifier;
        }
        velocity.y = velocity.y > jumpForce * scaleModifier ? jumpForce * scaleModifier : velocity.y;
        rb2d.velocity = velocity;
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

    private void OrientPlayer()
    {
        if ((moveHorizontal > 0 && !facingRight) || (moveHorizontal < 0 && facingRight)){
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}
