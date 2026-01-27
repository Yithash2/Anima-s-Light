using System;
using System.Collections;
using System.Collections.Generic;
using Levels;
using UnityEngine;

public class CustomPhysicsMaterials : MonoBehaviour
{
    public GroundMaterial material;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerStateMachine.Instance.GroundType = material;
        }
    }
}
