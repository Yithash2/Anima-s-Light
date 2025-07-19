using Dialogue;
using System;
using UnityEngine;

public class ForcedInteraction : DialogInteractions
{
    public override ImportanceOrder Importance => ImportanceOrder.AlwaysFirst;
    public override bool CanBeSelected => false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isInDialogue || !_canInteract) return;

        _canInteract = false;
        Manager.Subscribe(this);
        Manager.StartInteraction(this);
    }
    private enum DestroyMode {none, start, end}
    [SerializeField] private DestroyMode destroyAtTheEnd = DestroyMode.none;
    [SerializeField] private GameObject objectToDestroy;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_canRestartDialogue) _canInteract = true;
    }

    public override void OnInteractionEnd()
    {
        Manager.Unsubscribe(this);
        _isInDialogue = false;
        if(destroyAtTheEnd == DestroyMode.end)Destroy(objectToDestroy);
        
        
    }

    public override void OnInteractionStart()
    {
        DialogManager.Instance.StartDialog(_dialog);
        _isInDialogue = true;
        
        if(destroyAtTheEnd == DestroyMode.start)Destroy(objectToDestroy);
    }
}

