using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Teleporter : Interactable
{
    [SerializeField] Transform transportLocation;
    public GameObject Location { set { transportLocation = value.transform; } }
    public float Height { set { transform.localScale = new Vector3(transform.localScale.x, value/16, transform.localScale.z); transform.position += Vector3.up * value / 32f; } }
    public float Width { set { transform.localScale = new Vector3(value/16, transform.localScale.y, transform.localScale.z); transform.position += Vector3.right * value / 32f; } }
    
    

    bool _isSubscribed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Manager.Subscribe(this);
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
        Teleport();
        Manager.StopInteraction();
    }

    void Teleport()
    {
        var _player = PlayerStateMachine.Instance;
        // Calculate the offset of the player relative to the PlayerHolder
        Vector3 playerOffset = _player.transform.localPosition;

        // Move PlayerHolder to the target location, keeping the player's offset intact
        Vector3 targetPosition = transportLocation.position;
        _player.transform.parent.position = targetPosition - playerOffset;
    }
}

public struct SortedPool
{
    private List<Interactable> _values;
    public void RemoveValue(Interactable value)
    {
        if (_values.Contains(value))
        {
            _values.Remove(value);
            return;
        }

        throw new InvalidOperationException($"The value '{value}' was not found and could not be removed.");
    }

    public void AddValue(Interactable value)
    {
        _values.Add(value);
    }

    public Interactable Get(InteractableManager manager)
    {
        var index = 0;
        for (int i = 1; i < _values.Count - 1; i++)
        {
            var distInteract = Vector3.Distance(_values[i].transform.position, manager.transform.position);
            var distCurr = Vector3.Distance(_values[index].transform.position, manager.transform.position);
            if (distInteract < distCurr)
            {
                index = i;
            }else if (Mathf.Approximately(distInteract, distCurr))
            {
                if (_values[i].Importance > _values[index].Importance)
                {
                    index = i;
                }
                else
                {
                    break;
                }
                
            }
        }
        return _values[index];
    }
    
    public Interactable Dequeue(InteractableManager manager)
    {
        var index = 0;
        for (int i = 1; i < _values.Count - 1; i++)
        {
            var distInteract = Vector3.Distance(_values[i].transform.position, manager.transform.position);
            var distCurr = Vector3.Distance(_values[index].transform.position, manager.transform.position);
            if (distInteract < distCurr)
            {
                index = i;
            }else if (Mathf.Approximately(distInteract, distCurr))
            {
                if (_values[i].Importance > _values[index].Importance)
                {
                    index = i;
                }
                else
                {
                    break;
                }
                
            }
        }
        
        var value = _values[index];
        _values.RemoveAt(index);
        return value;
    }

    public bool IsEmpty()
    {
        return _values.Count == 0;
    }

    public SortedPool(int i)
    {
        _values = new List<Interactable>();
    }
}
