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
    [SerializeField, Range(0, 1)]
    float _friction;
    
    [SerializeField]
    SpriteRenderer _sprite;

    private Vector2 _direction;


    [SerializeField] bool isLooping;

    void FixedUpdate()
    {

        if (Vector3.Distance(transform.position, pointsToReach[_currentTargetPoint].transform.position) <= 0.5f + Mathf.Sqrt(platformSpeed * Time.deltaTime) && !IsWaiting)
        {
            StartCoroutine(WaitForSeconds());
        }
        else if (!IsWaiting)
        {
            _direction = pointsToReach[_currentTargetPoint].transform.position - transform.position;
            transform.position = Vector3.MoveTowards(transform.position, pointsToReach[_currentTargetPoint].transform.position, platformSpeed * Time.fixedDeltaTime);
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

    IEnumerator WaitForSeconds()
    {
        IsWaiting = true;
        StartCoroutine(MomentumStorage()); // Start the momentum storage coroutine
        yield return new WaitForSeconds(pointsToReach[_currentTargetPoint].WaitTime);
        _currentTargetPoint = GetcurrentIndex(_currentTargetPoint);
        IsWaiting = false;
    }

    IEnumerator MomentumStorage()
    {
        _sprite.color = Color.cyan;  // Change color to indicate waiting
        yield return new WaitForSeconds(1f); // Wait for 0.2 seconds
        _sprite.color = Color.white; // Reset the color
        _direction = Vector2.zero;   // Reset the direction
    }


    void OnCollisionEnter2D(Collision2D collision){
        if(_friction != 0)collision.transform.parent.parent = transform;

    }

    void OnCollisionStay2D(Collision2D collision){

    }

    void OnCollisionExit2D(Collision2D collision){
        if (_friction == 0) return;

            collision.transform.parent.parent = null;
            if (collision.gameObject.CompareTag("Player")){
                PlayerStateMachine PSM = PlayerStateMachine.Instance;
                if(IsDashing(PSM) ||IsJumping(PSM)){
                    if(_direction.normalized.y > 0)PSM.Rb.linearVelocity += _direction.normalized * platformSpeed * _friction;
                    else if(_direction.normalized.x != 0 && !PSM.IsWallJumping) PSM.Rb.linearVelocity += new Vector2(_direction.normalized.x - 1, 0) * platformSpeed * _friction;
                }
            }
          
    }

    bool IsDashing(PlayerStateMachine PSM){
        return PSM.IsDashing;
    }

    bool IsJumping(PlayerStateMachine PSM){
        return PSM.JumpPressed || PSM.IsJumping || PSM.IsWallJumping;
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
