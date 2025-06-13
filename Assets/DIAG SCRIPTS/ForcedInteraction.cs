using Dialogue;
using System;
using UnityEngine;

public class ForcedInteraction : DialogInteractions
{
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isInDialogue || !_canInteract) return;

        _canInteract = false;
        DialogManager.Instance.StartDialog(_dialog, this);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_canRestartDialogue) _canInteract = true;
    }

    public override void OnDialogueEnd()
    {
        _isInDialogue = false;
    }

    public override void OnDialogueStart()
    {
        _isInDialogue = true;
    }
}

