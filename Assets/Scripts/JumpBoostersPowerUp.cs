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
    private float t = 0f;
    private float resetCounter = 0f;
    private CircleCollider2D _collider;
    private float _radius;

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

            if (PlayerIsAbove())
            {
                Player.Rb.velocity = new Vector2(Player.Rb.velocity.x, Player.Rb.velocity.y / 2f);
                Player.JumpCount = 0;
                StartCoroutine(JumpAuthorized());
                
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag(playerTag) && _isActivated)
        {    
            if(Player.Rb.velocity.y < 0){
                _isActivated = false;
                resetCounter = 0f; // Reset resetCounter on activation
            }
            
        }
    }

    IEnumerator SpeedFreeze(){
        float _realTilemScale = 1;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(0.3f);
        Time.timeScale = _realTilemScale;
    }

    IEnumerator JumpAuthorized()
    {
        t = 0f;

        while (t < JumpTime)
        {
            Player.CoyoteCounter = 0;

            if (Player.JumpCount != 0)
            {
                Player.JumpCount = 1;
                if(Mathf.Abs(Player.Rb.velocity.x) >= 40f){
                    StartCoroutine(SpeedFreeze());
                }
                _isActivated = false;
                resetCounter = 0f; // Reset resetCounter on activation
                break; // Exit loop if jump is authorized or player falls
            }

            t += Time.deltaTime;
            yield return null;
        }

        // Reset CoyoteCounter to an appropriate value after the loop ends
        Player.CoyoteCounter = 1f; // Set to a suitable value after the jump ends
    }

    bool PlayerIsAbove()
    {
        return Player.transform.position.y > transform.position.y;
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
            UpdateColliderRadius();
        }
    }

    void ActivatePowerUp()
    {
        _isActivated = true;
        SpriteRenderer.sprite = ActivatedSprite;
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
            Handles.Label(transform.position + new Vector3(0, 4, 0), t.ToString());
        #endif
    }
}
