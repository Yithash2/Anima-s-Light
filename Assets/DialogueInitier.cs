using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInitier : MonoBehaviour
{
    bool _hasBeenUsed;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_hasBeenUsed) return;

        GetComponent<Dialogue>().NextLine();
        _hasBeenUsed = true;
    }
}
