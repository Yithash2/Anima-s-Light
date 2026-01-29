using UnityEngine;

public class CloneTrail : MonoBehaviour
{
    private Color originalColor;
    private Color targetColor;
    private float lerpColorTimer = 0f;
    private bool isSpawningClones = false;

    [SerializeField] private GameObject _motionBlurTwins;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Rigidbody2D _rb;
    private FXManager _fxManager; 

    void Start()
    {
        _fxManager = FXManager.Instance;
        originalColor = _sprite.color;
        targetColor = _motionBlurTwins.GetComponent<SpriteRenderer>().color;
    }

    void FixedUpdate()
    {
        float _maxDistance = 0.8f;
        float velocityX = Mathf.Abs(_rb.linearVelocity.x);
        float velocityY = Mathf.Abs(_rb.linearVelocity.y);
        
        // Check if the velocity exceeds thresholds to start spawning clones
        if (velocityX > 40 || velocityY > 45) 
        {
            if (!isSpawningClones)
            {
                lerpColorTimer = 0f;
                isSpawningClones = true;
            }

            // Instantiate a new clone and set it up
            GameObject clone = Instantiate(_motionBlurTwins, transform.position, Quaternion.identity);
            SpriteRenderer spriteRenderer = clone.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = _sprite.sprite;
            spriteRenderer.flipX = transform.localScale.x > 0;

            // Set the color to the target color for a motion blur effect
            _sprite.color = targetColor;

            // Manage effects
            _fxManager.LerpHue(0);

            // Destroy the clone after 0.1 seconds
            Destroy(clone, 0.1f);
        }
        else
        {
            // Stop spawning clones if velocity is low
            if (isSpawningClones)
            {
                isSpawningClones = false;
                lerpColorTimer = 0f;
            }

            // Lerp the sprite color back to original if needed
            if (_sprite.color != originalColor)
            {
                lerpColorTimer += Time.deltaTime;
                _sprite.color = Color.Lerp(targetColor, originalColor, lerpColorTimer);
                _fxManager.LerpHue(Mathf.Clamp01(lerpColorTimer * 2));
            }
        }
    }
}
