using UnityEngine;

public class ColorFade : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color initialColor;
    private float fadeDuration = 0.1f;
    private float timer;

    private void Start()
    {
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
            spriteRenderer.color = Color.Lerp(initialColor, Color.black, t);
        }
    }

    public void SetFadeDuration(float duration)
    {
        fadeDuration = duration;
    }
}
