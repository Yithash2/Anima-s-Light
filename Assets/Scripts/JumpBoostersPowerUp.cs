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
    CircleCollider2D _collider;

    float resetCounter;

    void Start(){
        Player = PlayerStateMachine.Instance;
        _collider = GetComponent<CircleCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag(playerTag) && _isActivated){
            Player.BufferCounter = 0;

            if(PlayerIsAbove())  {
                Player.Rb.velocity = new Vector2(Player.Rb.velocity.x, 0);
                Player.CoyoteCounter = 0;
                Player.JumpCount = 0;
                _isActivated = false;
            }
        }
    }

    bool PlayerIsAbove(){
        return Player.transform.position.y > transform.position.y;
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
