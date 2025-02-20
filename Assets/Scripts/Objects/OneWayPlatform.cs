using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    PlayerStateMachine player;

    LayerMask _platformActive,
    _platformNonActive;

    bool _coroutineActive;
    Collider2D _collider;

    [SerializeField] bool _isPassable = true;

    void Start()
    {
        player = PlayerStateMachine.Instance;
        _platformActive = LayerMask.NameToLayer("Ground");
        _platformNonActive = LayerMask.NameToLayer("Default");
        _collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.position) > 30)
        {
            // If the player is far, disable collision and change layer to non-active
            _collider.enabled = true;
            gameObject.layer = _platformNonActive;
        }
        else
        {
            if (_isPassable && player.DivePressed)
            {
                // If player is diving, allow passing through by making collider a trigger
                gameObject.layer = _platformNonActive;
                _collider.isTrigger = true;
                return;
            }
            else
            {
                if (!_coroutineActive && _collider.isTrigger)
                    StartCoroutine(WaitUntilPlayerIsUnder());  // Start coroutine when player is under the platform
                else if (!_coroutineActive)
                {
                    // Check if player is above and handle the platform's layer accordingly
                    if (PlayerIsAbove())
                    {
                        gameObject.layer = _platformActive;
                    }
                    else if (gameObject.layer == _platformActive)
                    {
                        // Change to non-active layer if the player moves away
                        gameObject.layer = _platformNonActive;
                    }
                }
            }
        }
    }

    bool PlayerIsAbove()
    {
        // Check if the player is above the platform and falling
        return player.Rb.velocity.y <= 0 && player.transform.position.y > (transform.position.y + 1);
    }

    IEnumerator WaitUntilPlayerIsUnder()
    {
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        _coroutineActive = true;

        // Wait until the player moves below the platform (touching condition)
        while (_collider.IsTouching(playerCollider))
        {
            yield return null;
        }

        // Once the player is no longer touching, make the collider no longer a trigger
        _collider.isTrigger = false;
        _coroutineActive = false;
    }
}
