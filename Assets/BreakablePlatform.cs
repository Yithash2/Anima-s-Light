using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    PlayerStateMachine player;

    LayerMask _platformActive,
    _platformNonActive;
    Collider2D _collider;

    [SerializeField] GameObject _destoyParticles;


    void Start()
    {
        player = PlayerStateMachine.Instance;
        _platformActive = LayerMask.NameToLayer("Ground");
        _platformNonActive = LayerMask.NameToLayer("Default");
        _collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        
        if (player.Rb.linearVelocity.y < -45){
            gameObject.layer = _platformNonActive;
            _collider.isTrigger = true;
            return;
        }

        gameObject.layer = _platformActive;
        _collider.isTrigger = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(player.Rb.linearVelocity.y < -45 && collision.gameObject == player.gameObject)
        {
            Instantiate(_destoyParticles, transform.position, _destoyParticles.transform.rotation);
            Destroy(gameObject, 0.05f);
        }
    }

}
