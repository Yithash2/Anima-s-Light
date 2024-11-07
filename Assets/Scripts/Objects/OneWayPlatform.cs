using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    PlayerStateMachine player;

    LayerMask _platformActive,
    _platformNonActive;

    bool _coroutineActive;
    Collider2D _collider;

    void Start(){
        player = PlayerStateMachine.Instance;
        _platformActive = LayerMask.NameToLayer("Ground");
        _platformNonActive = LayerMask.NameToLayer("Default");
        _collider = GetComponent<Collider2D>();
    }
    void Update()
    {   
        if(Vector2.Distance(player.transform.position, transform.position) > 30){
            _collider.enabled = true;
            gameObject.layer =_platformNonActive;
        }else{
            if(player.DivePressed){
                gameObject.layer =_platformNonActive;
                _collider.isTrigger = true;
                return;
            }else{

                if(!_coroutineActive && _collider.isTrigger) StartCoroutine(WaitUntilPlayerIsUnder());
                else if(!_coroutineActive){
                    if(PlayerIsAbove()){
                        gameObject.layer = _platformActive;
                    }else if(gameObject.layer == _platformActive){
                        gameObject.layer =_platformNonActive;
                    }
                }
                
            }       
        }
    }

    bool PlayerIsAbove(){
        return player.Rb.velocity.y <= 0 && player.transform.position.y > transform.position.y;
    }

    IEnumerator WaitUntilPlayerIsUnder(){

        Collider2D playerCollider = player.GetComponent<Collider2D>();
        _coroutineActive = true;

        
        while(_collider.IsTouching(playerCollider)){
            yield return null;
        }

        _collider.isTrigger = false;
        _coroutineActive = false;
    }
}
