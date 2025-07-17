
using UnityEngine;

namespace Dialogue
{
    public abstract class DialogInteractions : Interactable
    {
        public override bool CanBeSelected => true;

        protected bool _isInDialogue = false;
        protected bool _canInteract = true;

        [SerializeField] protected bool _canRestartDialogue;

        [SerializeField] protected Dialog _dialog;
    }

    
}
