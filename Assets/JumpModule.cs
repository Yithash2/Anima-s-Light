using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class JumpModule : MonoBehaviour
{
    private Rigidbody2D rb;
    public float initalJumpHeight;

    [Header("Jump Realase Type")]
    public bool divVelOnRelease;
    public float releaseDivFactor;

    [Header("Ground Check")]
    public Transform groundCheck;
    public Vector2 checkSize;
    public LayerMask groundLayer;

    [Header("Fast Fall")]
    public bool doFastFall;
    float gravityScale;
    public float gravityMult;
    public bool velocityIsClamped;
    public float clampedVelocity;

    [Header ("Timers")]
    public float bufferTime;
    float bufferCounter;

    //Inputs
    bool jumpPressed;
    bool jumpReleased;

    bool isJumping;

    // TODO: Head Checks

    // TODO: Wall Checks

    // TODO: WallJump

    // TODO: CoyoteTime

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gravityScale = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(velocityIsClamped && rb.velocity.y < 0) clampVelocity();

        if(doFastFall) FastFall();

        if(Grounded()){
            isJumping = false;
        }

        Inputs();
        
    }

    void Inputs(){

        if(Input.GetKeyDown(KeyCode.Space)) jumpPressed = true;

        if(jumpPressed && !Grounded()){
            if(bufferCounter < bufferTime){
                bufferCounter += Time.deltaTime;
            }else{
                jumpPressed = false;
            }
        }
        
        if(Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0 ) jumpReleased = true;
    }
    void clampVelocity(){
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -clampedVelocity, clampedVelocity));
    }
    void FastFall(){

        if(rb.velocity.y < 0){
            
            rb.gravityScale = gravityScale * gravityMult;

        }else{

            rb.gravityScale = gravityScale;

        }
    }

    void FixedUpdate(){

        Jump();

    }
    void Jump(){

        if(jumpPressed && Grounded() && !isJumping){

            bufferCounter = 0;
            jumpPressed = false;

            isJumping = true;

            rb.gravityScale = gravityScale;

            float jumpForce = Mathf.Sqrt(initalJumpHeight * -2 * (Physics2D.gravity.y * rb.gravityScale));
                
            rb.velocity = Vector2.up * jumpForce;

        }


        if(jumpReleased && rb.velocity.y > 0 && isJumping){

            jumpReleased = false;

            if(divVelOnRelease){

                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / releaseDivFactor);

            }else{

                rb.velocity = new Vector2(rb.velocity.x, 0);

            }

        }
    }

    public bool Grounded(){
        return Physics2D.OverlapBox(groundCheck.position, checkSize, 0, groundLayer);
    }
    void OnDrawGizmos(){
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(groundCheck.position, checkSize);
    }

}
