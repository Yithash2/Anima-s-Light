using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FellesShadow : MonoBehaviour
{
    
    [SerializeField] Animator _anim;

    SpriteRenderer _spr;
    CircleCollider2D _cCollider;

    [SerializeField] GameObject SpawnedObject;

    public bool IsFlipped { set { _spr.flipX = value; } }

    public float Range { set { _cCollider.radius = value; } }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        _anim.Play("Felle's Shadow Decomposing");
    }

    public void DestrySelf()
    {
        if(SpawnedObject != null)
        {
            Instantiate(SpawnedObject, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void OnValidate()
    {
        _spr = GetComponent<SpriteRenderer>();
        _cCollider = GetComponent<CircleCollider2D>();
    }
}
