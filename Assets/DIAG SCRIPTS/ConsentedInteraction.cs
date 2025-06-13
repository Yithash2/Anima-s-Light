using Dialogue;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConsentedInteraction : DialogInteractions
{
    [SerializeField] private GameObject _canInteractVis;

    InputAction _interact;

    private bool _canStartDialogue = true;

    private void Start()
    {
        _interact = InputsManager.Instance.Dive;
        _interact.started += Interact;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isInDialogue) return;

        if (!_canStartDialogue && !_canRestartDialogue) return;
        
        _canInteract = true;
        _canInteractVis.SetActive(_canInteract);
        
        
        
    }

    private void Interact(InputAction.CallbackContext ctx)
    {
        if (!_isInDialogue && _canInteract)
        {
            DialogManager.Instance.StartDialog(_dialog, this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _canInteract = false;
        _canInteractVis.SetActive(_canInteract);
    }

    public override void OnDialogueEnd()
    {
        _isInDialogue = false;

        if (!_canRestartDialogue) {
            _canStartDialogue = false;
            _canInteractVis.SetActive(false);
            _canInteract = false;
        }
        else
        {
            _canInteractVis.SetActive(true);
            _canInteract = true;
        }
    }

    public override void OnDialogueStart()
    {
        _isInDialogue = true;
        _canInteractVis.SetActive(false);
    }
}
