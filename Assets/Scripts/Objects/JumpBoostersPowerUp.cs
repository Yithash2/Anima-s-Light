using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

public class JumpBoostersPowerUp : MonoBehaviour
{
    [SerializeField] SpriteRenderer SpriteRenderer;
    [SerializeField] Sprite ActivatedSprite;
    [SerializeField] Sprite DesactivatedSprite;

    public string playerTag;
    public float JumpTime = 2f;
    public float ResetTime;

    private PlayerStateMachine Player;
    private bool _isActivated = true;
    private float _jumpCounter = 0f;
    private float resetCounter = 0f;
    private CircleCollider2D _collider;
    private float _radius;
    private bool _playerBeenAbove;

    void Start()
    {
        Player = PlayerStateMachine.Instance;
        _collider = GetComponent<CircleCollider2D>();
        _radius = _collider.radius;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(playerTag) && _isActivated)
        {
            Player.BufferCounter = 0;
            //if (Player.Rb.velocity.y < 0) Player.Rb.velocity = new Vector2(Player.Rb.velocity.x, Player.Rb.velocity.y / 2f);
            Player.ResetJump();
            _jumpCounter = 0;
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag(playerTag) && _isActivated)
        {
            if(collider.bounds.min.y > transform.position.y) _playerBeenAbove = true;
            
            if (_jumpCounter < JumpTime)
            {
                Player.CoyoteCounter = 0;
                if (Player.JumpCount > 0)
                {
                    DeActivatePowerUp();
                }

                _jumpCounter += Time.deltaTime;
            }
            else
            {
                //Player.JumpCount = 1;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (_jumpCounter < JumpTime)
            {
                Player.CoyoteCounter = 0;
                if (Player.JumpCount > 0)
                {
                    DeActivatePowerUp();
                }

                _jumpCounter += Time.deltaTime;
            }
        }
    }
    


    void Update()
    {
        if (!_isActivated)
        {
            if (Player.tag == "Respawn")
            {
                resetCounter = ResetTime + 1;
                ActivatePowerUp();
            }
            else if (resetCounter < ResetTime)
            {
                resetCounter += Time.deltaTime;
                SpriteRenderer.sprite = DesactivatedSprite;
            }
            else
            {
                ActivatePowerUp();
            }
        }
        else
        {
            resetCounter = 0f;
            //UpdateColliderRadius();
        }
    }

    void ActivatePowerUp()
    {
        _isActivated = true;
        SpriteRenderer.sprite = ActivatedSprite;
    }

    void DeActivatePowerUp()
    {
        _isActivated = false;
        SpriteRenderer.sprite = DesactivatedSprite;
    }

    void UpdateColliderRadius()
    {
        if (Vector2.Distance(Player.transform.position, transform.position) <= 35)
        {
            _collider.radius = Player.Rb.velocity.y <= 0
                ? Mathf.Sqrt((1 / 160f) * (-Player.Rb.velocity.y)) + _radius
                : _radius;
        }
        else
        {
            _collider.radius = _radius;
        }
    }

    void OnDrawGizmos()
    {
        #if UNITY_EDITOR
            Handles.Label(transform.position + new Vector3(0, 4, 0), _jumpCounter.ToString());
        #endif
    }
}
