using System.Collections;
using UnityEngine;

public class ColorFade : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color initialColor;
    private float fadeDuration = 0.1f;

    [SerializeField]
    Color _targetColor;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            initialColor = spriteRenderer.color;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(ChangeColor());
    }

    private void OnDisable(){
        spriteRenderer.color = initialColor;
    }

    IEnumerator ChangeColor(){
        if (spriteRenderer != null)
        {
            float timer = 0;
            while(spriteRenderer.color != Color.black && gameObject.activeInHierarchy){
                timer += Time.deltaTime;
                float t = timer / fadeDuration; // Normalize the time
                spriteRenderer.color = Color.Lerp(initialColor, _targetColor, t);
                yield return 0f;
            }
            
        }
    }

}
