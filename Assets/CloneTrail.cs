using System.Collections.Generic;
using UnityEngine;

public class CloneTrail : MonoBehaviour
{
    private List<GameObject> clonePool = new List<GameObject>();
    private int poolSize = 5; // Define the number of clones you want in the pool
    private Color originalColor;
    private Color targetColor;
    private float lerpColorTimer = 0f;
    private bool isSpawningClones = false;
    private float cloneHideTimer = 0.1f; // Time before hiding clones
    private float currentCloneHideTime = 0f;

    [SerializeField] private GameObject _motionBlurTwins;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Rigidbody2D _rb;
     private FXManager _fxManager; 
    [SerializeField] private TrailRenderer _trailRenderer; 

    void Start()
    {
        _fxManager = FXManager.Instance;
        for (int i = 0; i < poolSize; i++)
        {
            GameObject clone = Instantiate(_motionBlurTwins, transform.position, Quaternion.identity);
            clone.SetActive(false); // Start each clone as inactive
            clonePool.Add(clone);
        }

        originalColor = _sprite.color;
        targetColor = _motionBlurTwins.GetComponent<SpriteRenderer>().color;
    }

    void FixedUpdate()
    {
        float _maxDistance = 0.8f;
        float velocityX = Mathf.Abs(_rb.velocity.x);
        float velocityY = Mathf.Abs(_rb.velocity.y);
        
        // Check if the velocity exceeds thresholds to start spawning clones
        if (velocityX > 40 || velocityY > 45) // Check magnitude instead of separate axes
        {
            if (!isSpawningClones)
            {
                lerpColorTimer = 0f;
                isSpawningClones = true;
            }

            // Get an inactive clone from the pool, if available
            GameObject clone = GetPooledClone(clonePool);
            if (clone != null)
            {
                float smoothTime = Mathf.Clamp(Vector2.Distance(clone.transform.position, transform.position)/_maxDistance, 0, 1);
                Vector3 targetPosition = transform.position;
                clone.transform.position = Vector3.Lerp(clone.transform.position, targetPosition, smoothTime); // Adjust speed of interpolation

                clone.SetActive(true);
                SpriteRenderer spriteRenderer = clone.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = _sprite.sprite;
                spriteRenderer.flipX = transform.localScale.x > 0;

                // Set the color to the target color for a motion blur effect
                _sprite.color = targetColor;

                // Manage effects
                _fxManager.LerpHue(0);
                _trailRenderer.time = 0.1f;

                // Reset the hide timer for the clone
                currentCloneHideTime = cloneHideTimer;
            }
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
                _trailRenderer.time = 0f;
            }
        }

        // Manage hiding of active clones
        if (currentCloneHideTime > 0)
        {
            currentCloneHideTime -= Time.deltaTime;

            // Hide clones if their time has expired
            foreach (GameObject clone in clonePool)
            {
                if (clone.activeInHierarchy && currentCloneHideTime <= 0)
                {
                    clone.SetActive(false);
                }
            }
        }
    }

    private GameObject GetPooledClone(List<GameObject> clonePool)
    {
        // Find an inactive clone in the pool
        foreach (GameObject clone in clonePool)
        {
            if (!clone.activeInHierarchy)
            {
                return clone;
            }
        }
        return null; // No inactive clones available
    }
}
