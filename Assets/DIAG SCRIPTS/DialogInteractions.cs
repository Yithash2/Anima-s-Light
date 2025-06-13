
using UnityEngine;

namespace Dialogue
{
    public abstract class DialogInteractions : MonoBehaviour
    {
        public abstract void OnDialogueEnd();
        public abstract void OnDialogueStart();

        protected bool _isInDialogue = false;
        protected bool _canInteract = true;

        [SerializeField] protected bool _canRestartDialogue;

        [SerializeField] protected Dialog _dialog;
    }

    
}
