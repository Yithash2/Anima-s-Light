using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Blockers : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spr;
    [SerializeField] float _maxDistanceRequired = 4f;
    [SerializeField] float _minDistanceRequired = 1f;
    Transform _player;
    void Start()
    {
        _player = PlayerStateMachine.Instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float _distance = Vector2.Distance(_player.position, transform.position);

        // Clamp the distance between the min and max distance to prevent out-of-range values
        float clampedDistance = Mathf.Clamp(_distance, _minDistanceRequired, _maxDistanceRequired);

        float normalizedDistance = Mathf.InverseLerp(_maxDistanceRequired, _minDistanceRequired, clampedDistance);
        // Normalize the distance based on the range [min, max]
        float transparency = Mathf.SmoothStep(0f, 1f, normalizedDistance);


        // Set the transparency based on the normalized distance
        var color = new Color(_spr.color.r, _spr.color.g, _spr.color.b, transparency);
        _spr.color = color;
    }


    private void OnValidate()
    {
        if (_spr == null) _spr = GetComponent<SpriteRenderer>();
    }
}
