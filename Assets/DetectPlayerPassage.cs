using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayerPassage : MonoBehaviour
{
    public Action OnEnougthPlayerPassage;

    [SerializeField] int _numberOfPassageTodoSmth;

    private int _numberOfPassage = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _numberOfPassage++;

            if(_numberOfPassage == _numberOfPassageTodoSmth)
            {
                OnEnougthPlayerPassage.Invoke();
            }
        }
    }
}
