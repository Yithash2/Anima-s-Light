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
    float _radius;


    float resetCounter;

    void Start(){
        Player = PlayerStateMachine.Instance;
        _collider = GetComponent<CircleCollider2D>();
        _radius = _collider.radius;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag(playerTag) && _isActivated){
            Player.BufferCounter = 0;

            if(PlayerIsAbove())  {
                Player.Rb.velocity = new Vector2(Player.Rb.velocity.x, Player.Rb.velocity.y/5f);
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

                if(Vector2.Distance(Player.transform.position, transform.position) <= 35){
                    if(Player.Rb.velocity.y <= 0) _collider.radius = Mathf.Sqrt((1/160f)*(-Player.Rb.velocity.y))+_radius;
                    else _collider.radius = _radius;
                }

            }

    }



}
