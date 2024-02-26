using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpModule : MonoBehaviour
{
    private Rigidbody2D rb;
    public float wallJumpHeight;
    public float wallJumpLength;
    
    [Header("Wall Check")]
    public Transform wallCheck;
    public Vector2 wallSize;
    public LayerMask wallLayer;

    [Header("Timers")]
    public float bufferTime;
    float bufferCounter;

    bool jumpPressed;
    bool jumpReleased;

    public JumpModule JM;

    void Start(){

        rb = GetComponent<Rigidbody2D>();

    }
    void Update(){

        if(Input.GetKeyDown(KeyCode.Space) && Walled()){
            WallJump();
        }

        //Inputs();

    }

    void WallJump(){

        Debug.Log("IM JUMPING");

        bufferCounter = 0;
        jumpPressed = false;
        
        float verticalJumpForce = Mathf.Sqrt(wallJumpHeight * -2 * (Physics2D.gravity.y * rb.gravityScale));
        float timeToPeak = verticalJumpForce / Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
        float totalTimeInAir = 2 * timeToPeak;
        float horizontalJumpForce = wallJumpLength / totalTimeInAir;


        rb.velocity = new Vector2(horizontalJumpForce , verticalJumpForce);

    }

        /*void Inputs(){

        if(Input.GetKeyDown(KeyCode.Space)) jumpPressed = true;

        if(jumpPressed){
            if(bufferCounter < bufferTime){
                bufferCounter += Time.deltaTime;
            }else{
                jumpPressed = false;
            }
        }
        
        if(Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0 ) jumpReleased = true;
    }*/

    bool Walled(){

        return Physics2D.OverlapBox(wallCheck.position, wallSize, 0, wallLayer);
        
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(wallCheck.position, wallSize);
    }
}
