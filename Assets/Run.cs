using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class Run : MonoBehaviour
{
    public float speed;
    Rigidbody2D rb;
    public static bool isFacingRight;

    public JumpModule JM;

    [SerializeField] Transform playerTransdorm;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    void Update(){

        float horizontal = Input.GetAxisRaw("Horizontal");
        
        if(horizontal != 0) rb.velocity += new Vector2(speed * horizontal, 0);

        if(isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            Flip(playerTransdorm);
            
        }
    }

    public static void Flip(Transform playerTransform){
        
            isFacingRight = !isFacingRight;
            Vector3 theScale = playerTransform.localScale;
            theScale.x *= -1f;
            playerTransform.localScale = theScale;
        
    }
}
