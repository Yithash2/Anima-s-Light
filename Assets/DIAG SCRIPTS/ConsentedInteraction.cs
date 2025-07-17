using Dialogue;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConsentedInteraction : DialogInteractions
{
    private bool _canStartDialogue = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isInDialogue) return;

        if (!_canStartDialogue && !_canRestartDialogue) return;
        
        Manager.Subscribe(this);
    
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (subscribed) Manager.Unsubscribe(this);
    }

    public override void OnInteractionEnd()
    {
        _isInDialogue = false;

        if (!_canRestartDialogue) {
            _canStartDialogue = false;
            _canInteract = false;
        }
        else
        {
            _canInteract = true;
        }
    }

    public override void OnInteractionStart()
    {
        DialogManager.Instance.StartDialog(_dialog);
        _isInDialogue = true;
    }
}
