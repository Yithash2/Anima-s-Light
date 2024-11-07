using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CloudIddle : MonoBehaviour
{
    private Collider2D _collider;
    private Vector3 _startPos;
    private Vector2 _startColOffset;

    [SerializeField]
    private float _timeOffset, _cloudDisplacement, _displacementSpeed;
    void Start()
    {
        _collider = GetComponent<Collider2D>();
        _startColOffset = _collider.offset;
        _startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var sinTime = Mathf.Sin(Time.time * _displacementSpeed + _timeOffset) * _cloudDisplacement;
        transform.position = new Vector3(0,sinTime , 0) + _startPos;
        _collider.offset = new Vector2(0, -sinTime) + _startColOffset;
    }
}
