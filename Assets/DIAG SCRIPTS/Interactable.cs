
    using System;
    using UnityEngine;
    using UnityEngine.Serialization;

    public abstract class Interactable : MonoBehaviour
    {
        
        public virtual ImportanceOrder Importance => ImportanceOrder.Mid;
        public virtual bool CanBeSelected => false;
        protected InteractableManager Manager {get; private set; }
        public abstract void OnInteractionEnd();
        public abstract void OnInteractionStart();

        public bool subscribed = false;

        public virtual void Start()
        {
            Manager = InteractableManager.Instance;
        }
    }
    
    public enum ImportanceOrder
    {
        AlwaysFirst = 0,
        Mid = 1,
        AlwaysLast = 2
    }
