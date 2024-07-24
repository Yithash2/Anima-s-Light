using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{

    [SerializeField] Transform[] pointsToReach;
    [HideInInspector] public List<Vector3> pTRvects;
    [SerializeField] float platformSpeed;
    [SerializeField] int _currentTargetPoint = 0;
    private bool isOnReturn;
    [SerializeField] Rigidbody2D RB;

    [SerializeField] float WaitTime;
    float _waitCounter;

    [SerializeField] bool _allowPlayerXDisplacement;
    [SerializeField] bool _allowPlayerYDisplacement;
    [SerializeField] bool isLooping;

    void OnValidate(){
        pTRvects.Clear();

        foreach(Transform t in pointsToReach){
            pTRvects.Add(t.position);
        }

        if(isLooping) pTRvects.Add(pointsToReach[0].position);
    }
    void Update()
    {

        if(Vector3.Distance(transform.position, pointsToReach[_currentTargetPoint].position) <= 0.5 + Mathf.Sqrt(platformSpeed * Time.deltaTime)){
            _currentTargetPoint = GetcurrentIndex(_currentTargetPoint);
            _waitCounter = 0;
        }

        if(_waitCounter <= WaitTime){
            RB.velocity = Vector2.zero;
            _waitCounter += Time.deltaTime;
        }else if(_waitCounter > WaitTime){
            Vector3 direction = pointsToReach[_currentTargetPoint].position - transform.position;
            RB.velocity = direction.normalized * platformSpeed;
            }
    }

    int GetcurrentIndex(int currentPoint){

        if(isLooping){
            if(currentPoint == pointsToReach.Length - 1){
                return 0;
            }else{
                return currentPoint + 1;
            }
        }else{
            
            if(currentPoint == pointsToReach.Length - 1){
                isOnReturn = true;
                
            }else if(currentPoint == 0){
                isOnReturn = false;
                
            }
            
            if(!isOnReturn){
                return currentPoint + 1;
            }else{
                return currentPoint - 1;
            }

        }

        
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.GetComponent<PlayerStateMachine>() != null && (_allowPlayerXDisplacement || _allowPlayerYDisplacement)){
            PlayerStateMachine PSM = collision.gameObject.GetComponent<PlayerStateMachine>();
            if(_allowPlayerXDisplacement || _allowPlayerYDisplacement)PSM.IsOnMovingPlatform = true;
        }
    }

    void OnCollisionStay2D(Collision2D collision){

        if(collision.gameObject.GetComponent<PlayerStateMachine>() != null && (_allowPlayerXDisplacement || _allowPlayerYDisplacement)){
            PlayerStateMachine PSM = collision.gameObject.GetComponent<PlayerStateMachine>();
            if(_allowPlayerXDisplacement)PSM.PlatformVelocityX = RB.velocity.x;
            if(_allowPlayerYDisplacement)PSM.PlatformVelocityY = RB.velocity.y;
        }
    }

    void OnCollisionExit2D(Collision2D collision){
        if(collision.gameObject.GetComponent<PlayerStateMachine>() != null && (_allowPlayerXDisplacement || _allowPlayerYDisplacement)){
            PlayerStateMachine PSM = collision.gameObject.GetComponent<PlayerStateMachine>();
            if(_allowPlayerXDisplacement || _allowPlayerYDisplacement)PSM.IsOnMovingPlatform = false;
        }

    }

    void OnTriggerEnter2D(Collider2D collider){
        Rigidbody2D playerRb = collider.gameObject.GetComponent<Rigidbody2D>();
        if(playerRb != null && playerRb.bodyType != RigidbodyType2D.Static){ 

            if(collider.gameObject.GetComponent<PlayerStateMachine>() != null){
                PlayerStateMachine PSM = collider.gameObject.GetComponent<PlayerStateMachine>();
                if(_allowPlayerXDisplacement || _allowPlayerYDisplacement)PSM.IsOnMovingPlatform = true;
            }

        }
    }

    void OnTriggerStay2D(Collider2D collider){
        Rigidbody2D playerRb = collider.gameObject.GetComponent<Rigidbody2D>();
        if(playerRb != null && playerRb.bodyType != RigidbodyType2D.Static){ 

            if(collider.gameObject.GetComponent<PlayerStateMachine>() != null){
                PlayerStateMachine PSM = collider.gameObject.GetComponent<PlayerStateMachine>();
                if(_allowPlayerXDisplacement)PSM.PlatformVelocityX = RB.velocity.x;
                if(_allowPlayerYDisplacement)PSM.PlatformVelocityY = RB.velocity.y;
            }

        }
    }

    void OnTriggerExit2D(Collider2D collider){
        Rigidbody2D playerRb = collider.gameObject.GetComponent<Rigidbody2D>();
        if(playerRb != null && playerRb.bodyType != RigidbodyType2D.Static){ 

            if(collider.gameObject.GetComponent<PlayerStateMachine>() != null){
                PlayerStateMachine PSM = collider.gameObject.GetComponent<PlayerStateMachine>();
                if(_allowPlayerXDisplacement || _allowPlayerYDisplacement)PSM.IsOnMovingPlatform = false;
            }

        }
    }

    void OnDrawGizmos(){
        for(int i = 0; i < pTRvects.ToArray().Length-1; i++){
            Gizmos.DrawLine(pTRvects.ToArray()[i], pTRvects.ToArray()[i+1]);
        }
        
            
    }
}
