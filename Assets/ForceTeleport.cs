using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceTeleport : Interactable
{
    [SerializeField] private Transform target;
    
    public override bool CanBeSelected => false;
    public override ImportanceOrder Importance => ImportanceOrder.AlwaysFirst;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Manager.Subscribe(this);
        Manager.StartInteraction(this);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(subscribed) Manager.Unsubscribe(this);
    }

    public override void OnInteractionEnd()
    {
        
    }

    public override void OnInteractionStart()
    {
        PlayerStateMachine.Instance.Teleport(target);
        Manager.StopInteraction();
    }
}
