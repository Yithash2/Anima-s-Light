using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FellesShadowDisapear : MonoBehaviour
{
    [SerializeField] Animator _anim;

    [SerializeField] GameObject SpawnedObject;
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
}
