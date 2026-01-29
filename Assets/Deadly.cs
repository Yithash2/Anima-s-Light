using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Deadly : MonoBehaviour
{
    [SerializeField] private bool nitPickDirection;   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            PlayerStateMachine.Instance.Die();
            
        }
    }

    private Vector2 CardinaliseNormal(Vector2 normal)
    {
        var dots = new float[4];
        var i = 0;
        foreach (Vector2 dir in CardinalVectors )
        {
            dots[i] = Vector2.Dot(dir, normal);
            i++;
        }

        var max = float.NegativeInfinity;
        i = 0;
        var index = 0;
        foreach (float dot in dots)
        {
            if (dot > max)
            {
                max = dot;
                index = i;
            }

            i++;
        }
        return CardinalVectors[index];
    }

    private bool IsInTheGoodDirection(Vector2 normal, Rigidbody2D rb)
    {
        return (normal.y == 0 || Mathf.Approximately(Mathf.Sign(rb.linearVelocity.y), Mathf.Sign(normal.y))) &&
               (normal.x == 0 || Mathf.Approximately(Mathf.Sign(rb.linearVelocity.x), Mathf.Sign(normal.x)));
    }

    private static readonly Vector2[] CardinalVectors = new []{Vector2.up, Vector2.right, Vector2.down, Vector2.left};
}
