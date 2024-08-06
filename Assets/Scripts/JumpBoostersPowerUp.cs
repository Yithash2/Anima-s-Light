using UnityEngine;
using UnityEngine.Lumin;

public class JumpBoostersPowerUp : MonoBehaviour
{
    [SerializeField] SpriteRenderer SpriteRenderer;

    [SerializeField] Sprite ActivatedSprite;
    [SerializeField] Sprite DesactivatedSprite;
    PlayerStateMachine Player;

    bool _isActivated = true;

    public string playerTag;

    public float ResetTime;

    float resetCounter;

    void Start(){
        Player = PlayerStateMachine.Instance;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag(playerTag) && _isActivated){

            if(Player.Rb.velocity.y < -2 ) {
                Player.Rb.velocity = new Vector2(Player.Rb.velocity.x, 0);
                Player.CoyoteCounter = 0;
                Player.BufferCounter = 0;
                Player.JumpCount = 0;
                _isActivated = false;
            }
        }
    }

    void Update(){

            if(!_isActivated){
                

                if(resetCounter <= ResetTime){
                    resetCounter += Time.deltaTime;
                    SpriteRenderer.sprite = DesactivatedSprite;
                }else{
                    _isActivated = true;
                    SpriteRenderer.sprite = ActivatedSprite;
                }
            }else{
                resetCounter = 0;
                SpriteRenderer.sprite = ActivatedSprite;

            }

    }



}
