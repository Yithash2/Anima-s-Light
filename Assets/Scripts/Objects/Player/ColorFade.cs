using UnityEngine;

public class ColorFade : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color initialColor;
    private float fadeDuration = 0.1f;
    private float timer = 0f;

    [SerializeField]
    private Color _targetColor;

    private void Start()
    {
        // Get the SpriteRenderer component and set the initial color
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            initialColor = spriteRenderer.color;
        }
    }

    private void Update()
    {
        if (spriteRenderer != null)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration; // Normalize the time

            // Lerp the color based on the timer
            spriteRenderer.color = Color.Lerp(initialColor, _targetColor, t);

            // Stop when the fade is complete and reset timer if needed
            if (t >= 1f)
            {
                timer = 0f; // Reset timer if you want to loop the fade
                initialColor = spriteRenderer.color; // Update initial color to current color
            }
        }
    }
}

