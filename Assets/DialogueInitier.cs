using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInitier : MonoBehaviour
{
    bool _hasBeenUsed;

    [SerializeField] Dialogue ThisDialog;

    [SerializeField] bool _deleteAfter;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_hasBeenUsed) return;

        ThisDialog.NextLine();
        _hasBeenUsed = true;

        if (_deleteAfter) Destroy(gameObject);
    }
}
