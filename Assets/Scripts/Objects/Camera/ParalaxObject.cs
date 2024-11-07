using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ParallaxObject : MonoBehaviour
{
    public Transform subject;
    Vector2 _startPosition;
    [SerializeField]
    Vector2 parallaxFactor;
    [SerializeField]
    Vector2 _offset;

    GameObject _target;
    
    float textureUnitSizeX;
    float textureUnitSizeY;

    void Start()
    {
        _startPosition = (Vector2)transform.localPosition;
        if(subject != null) _target = subject.gameObject;
        else _target = Camera.main.gameObject;

        // Get the size of the texture to calculate looping
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite != null)
        {
            textureUnitSizeX = spriteRenderer.sprite.bounds.size.x * transform.localScale.x; // For 3x tiling
            textureUnitSizeY = spriteRenderer.sprite.bounds.size.y * transform.localScale.y;
        }
    }

    void FixedUpdate()
    {
        Vector2 temp = new Vector2(_target.transform.localPosition.x * (1 - parallaxFactor.x), _target.transform.localPosition.y * (1 - parallaxFactor.y));
        Vector2 newPos = new Vector2(_target.transform.localPosition.x * parallaxFactor.x, transform.localPosition.y);
        newPos.y =Mathf.Lerp(transform.localPosition.y, _target.transform.localPosition.y , parallaxFactor.y) + _offset.y;
        // Apply the adjusted position with parallax effect
        transform.localPosition = _startPosition + new Vector2(newPos.x, newPos.y);

        if(temp.x > _startPosition.x + textureUnitSizeX) _startPosition.x += textureUnitSizeX;
        else if(temp.x < _startPosition.x - textureUnitSizeX) _startPosition.x -= textureUnitSizeX;

    }
}

