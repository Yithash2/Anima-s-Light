using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Teleporter : MonoBehaviour
{
    [SerializeField] Transform _transportLocation;

    public GameObject Location { set { _transportLocation = value.transform; } }
    PlayerStateMachine Player;

    bool _isEntering;

    bool _isSubscribed = false;

    private void Start()
    {
        Player = PlayerStateMachine.Instance;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _isEntering)
        {
            Teleport();
        }
    }

    void Enter(InputAction.CallbackContext ctx)
    {
        // Set _isEntering to true when the input starts
        if (ctx.started)
        {
            _isEntering = ctx.started;
            return;
        }

        // Reset _isEntering once the teleportation action is performed (this can be done if necessary)
        if (!ctx.performed)
        {
            _isEntering = false;
        }
    }

    private void Update()
    {
        if (Vector2.Distance(Player.transform.position, transform.position) < 10 && !_isSubscribed)
        {
            Debug.Log("Subscribed");
            InputsManager.Instance.Dive.started += Enter;
            InputsManager.Instance.Dive.canceled += Enter;
            _isSubscribed = true;
        }
        else if (Vector2.Distance(Player.transform.position, transform.position) > 20 && _isSubscribed)
        {
            Debug.Log("UnSubscribed");
            _isSubscribed = false;

            InputsManager.Instance.Dive.started -= Enter;
            InputsManager.Instance.Dive.canceled -= Enter;
        }
    }

    void Teleport()
    {
        // Calculate the offset of the player relative to the PlayerHolder
        Vector3 playerOffset = Player.transform.localPosition;

        // Move PlayerHolder to the target location, keeping the player's offset intact
        Vector3 targetPosition = _transportLocation.position;
        Player.transform.parent.position = targetPosition - playerOffset;

        // Reset _isEntering after teleportation to avoid multiple teleportations
        _isEntering = false;
    }
}
