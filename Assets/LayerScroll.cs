using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LayerScroll : MonoBehaviour
{
    [SerializeField]
    float _scrollSpeed = 1f;
    
    private float _textureUnitSizeX;
    private Vector3 _startPos;
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _startPos = transform.position;

        // Calculate the width of the texture in world units
        _textureUnitSizeX = _spriteRenderer.bounds.size.x/5;
    }

    void Update()
    {
        // Move the object to the left based on the scroll speed
        transform.position += Vector3.left * _scrollSpeed * Time.deltaTime;

        // Check if the object has moved past one texture unit size
        if (transform.position.x <= _startPos.x - _textureUnitSizeX)
        {
            // Reset the position to create a seamless loop
            transform.position = new Vector3(_startPos.x, transform.position.y, transform.position.z);
        }
    }
}

