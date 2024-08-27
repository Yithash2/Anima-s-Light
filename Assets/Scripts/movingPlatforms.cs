using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{

    [SerializeField] MovingPlatformPoints[] pointsToReach;
    private bool IsWaiting;
    [SerializeField] float platformSpeed;
    [SerializeField] int _currentTargetPoint = 0;
    private bool isOnReturn;
    [SerializeField] Rigidbody2D RB;

    float _waitCounter;

    public GameObject Player;

    [SerializeField] bool _allowPlayerXDisplacement;
    [SerializeField] bool _allowPlayerYDisplacement;
    [SerializeField] bool isLooping;

    void Update()
    {

        if(Vector3.Distance(transform.position, pointsToReach[_currentTargetPoint].transform.position) <= 0.5 + Mathf.Sqrt(platformSpeed * Time.deltaTime) && !IsWaiting){
            RB.velocity = Vector2.zero;
            StartCoroutine(WaitForSeconds());
            
        }else if(!IsWaiting){
            Vector3 direction = pointsToReach[_currentTargetPoint].transform.position - transform.position;
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

    IEnumerator WaitForSeconds(){
        IsWaiting = true;
        yield return new WaitForSeconds(pointsToReach[_currentTargetPoint].WaitTime);
        _currentTargetPoint = GetcurrentIndex(_currentTargetPoint);
        IsWaiting = false;
        
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.GetComponent<PlayerStateMachine>() != null && (_allowPlayerXDisplacement || _allowPlayerYDisplacement)){
            PlayerStateMachine PSM = collision.gameObject.GetComponent<PlayerStateMachine>();
            Player.transform.SetParent(transform);
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
            Player.transform.SetParent(null);
            if(_allowPlayerXDisplacement || _allowPlayerYDisplacement)PSM.IsOnMovingPlatform = false;
        }

    }

    void OnTriggerEnter2D(Collider2D collider){
        Rigidbody2D playerRb = collider.gameObject.GetComponent<Rigidbody2D>();
        if(playerRb != null && playerRb.bodyType != RigidbodyType2D.Static){ 

            if(collider.gameObject.GetComponent<PlayerStateMachine>() != null){
                Player.transform.SetParent(transform);
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
                Player.transform.SetParent(null);
                PlayerStateMachine PSM = collider.gameObject.GetComponent<PlayerStateMachine>();
                if(_allowPlayerXDisplacement || _allowPlayerYDisplacement)PSM.IsOnMovingPlatform = false;
            }

        }
    }

    void OnDrawGizmos(){
        for(int i = 0; i < pointsToReach.Length-1; i++){
            Gizmos.DrawLine(pointsToReach[i].transform.position, pointsToReach[i+1].transform.position);
        }
        
        if(isLooping){
            Gizmos.DrawLine(pointsToReach[0].transform.position, pointsToReach[pointsToReach.Length-1].transform.position);
        }
            
    }
}
