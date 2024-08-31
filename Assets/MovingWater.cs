using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWater : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;
    [SerializeField] BoxCollider2D _collider;

    [SerializeField] Vector3 _direction;
    [SerializeField] float _speed;

     [SerializeField] LayerMask _waterLayer;

    
    void OnValidate()
    {
        var shape = _particleSystem.shape;
        shape.scale = _collider.size;
        shape.position = _collider.offset;

        var vel = _particleSystem.velocityOverLifetime;
        Vector3 dir = _direction.normalized * _speed;
        vel.x = Mathf.Sqrt(dir.x);
        vel.y = Mathf.Sqrt(dir.y);
        vel.z = Mathf.Sqrt(dir.z);
        vel.orbitalYMultiplier = 0;
        vel.orbitalXMultiplier = 0;

        var em = _particleSystem.emission;
        if(_speed != 0 || _direction == Vector3.zero){
            em.rateOverTime = _speed*_speed;
        }else{
            em.rateOverTime = 1;
            vel.y = 2;
            vel.orbitalYMultiplier = -0.2f;
            vel.orbitalXMultiplier = -0.2f;

        }
    }

    // Update is called once per frame
    void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.IsTouchingLayers(_waterLayer)){
            StartCoroutine(AddforceAtEndOfFrame(collider.attachedRigidbody,_direction.normalized * _speed));
        }
    }

    IEnumerator AddforceAtEndOfFrame(Rigidbody2D rb, Vector2 force){
        yield return new WaitForEndOfFrame();
        rb.AddForce(force) ;
    }
}
