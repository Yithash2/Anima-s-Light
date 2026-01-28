using System;
using System.Collections.Generic;
using UnityEngine;
using Tool;
using UnityEngine.InputSystem;

public class ForcedTeleporter : Interactable
{
    [SerializeField] Transform transportLocation;
    public override ImportanceOrder Importance => ImportanceOrder.AlwaysFirst;
    public override bool CanBeSelected => false;
    
    public GameObject Location { set {transportLocation = value.transform; } }
    public float Height { set { transform.localScale = new Vector3(transform.localScale.x, value/16, transform.localScale.z); transform.position += Vector3.up * value / 32f; } }
    public float Width { set { transform.localScale = new Vector3(value/16, transform.localScale.y, transform.localScale.z); transform.position += Vector3.right * value / 32f; } }

    bool _isSubscribed;

    protected Timer _timer;
    private ForcedTeleporter _forcedTeleporter;

    public override void Start()
    {
        base.Start();

        _timer = new Timer(1);
        _forcedTeleporter = transportLocation.GetComponent<ForcedTeleporter>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(_timer.Ended()){
            Manager.Subscribe(this);
            Manager.StartInteraction(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }

    public override void OnInteractionEnd()
    {
        
        Debug.Log("OnInteractionEnd");
        _timer.Reset();
        
        if(_forcedTeleporter != null){
            _forcedTeleporter._timer.Reset();}
        
        if(subscribed) Manager.Unsubscribe(this);
    }

    public override void OnInteractionStart()
    {
        Teleport();
        
    }

    void Teleport()
    {
        var player = PlayerStateMachine.Instance;
        // Calculate the offset of the player relative to the PlayerHolder
        Vector3 playerOffset = player.transform.localPosition;

        // Move PlayerHolder to the target location, keeping the player's offset intact
        Vector3 targetPosition = transportLocation.position;
        player.transform.parent.position = targetPosition - playerOffset;
        
        Manager.StopInteraction();
    }
    
    
}

