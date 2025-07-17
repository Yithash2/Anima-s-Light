using Dialogue;
using System;
using UnityEngine;

public class ForcedInteraction : DialogInteractions
{
    public override ImportanceOrder Importance => ImportanceOrder.AlwaysFirst;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isInDialogue || !_canInteract) return;

        _canInteract = false;
        Manager.StartInteraction(this);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_canRestartDialogue) _canInteract = true;
    }

    public override void OnInteractionEnd()
    {
        _isInDialogue = false;
    }

    public override void OnInteractionStart()
    {
        DialogManager.Instance.StartDialog(_dialog);
        _isInDialogue = true;
    }
}

