using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FellesShadowDisapear : MonoBehaviour
{
    [SerializeField] Animator _anim;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _anim.Play("Felle's Shadow Decomposing");
    }

    public void DestrySelf()
    {
        Destroy(gameObject);
    }
}
