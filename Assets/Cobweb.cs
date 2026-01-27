using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Cobweb : MonoBehaviour
{
    
    public int NumPassage
    {
        set { allowedPassage = value; }
    }

    public bool CanFallThrough
    {
        set { stopsGravity = value; }
    }

    [SerializeField] private int allowedPassage = 1;
    private int _passageCounter = 0;

    [SerializeField] private bool stopsGravity = true;
    
    [SerializeField] SpriteRenderer sprRend;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        if ((allowedPassage!=-1 && _passageCounter >= allowedPassage)) return;

        StartCoroutine(StuckRoutine());

    }

    IEnumerator StuckRoutine()
    {
        var gS = PlayerStateMachine.Instance.Rb.gravityScale;
        
        if(stopsGravity)PlayerStateMachine.Instance.Rb.gravityScale = 0;
        
        PlayerStateMachine.Instance.Dies += Reset;
        PlayerStateMachine.Instance.Rb.velocity = new Vector2(0, 0);
        PlayerStateMachine.Instance.transform.position = transform.position;
        _passageCounter++;
        PlayerStateMachine.Instance.stuck = true;

        float t = 0;
        while (t < 0.5f && !PlayerStateMachine.Instance.DivePressed)
        {
            t += Time.deltaTime;
            yield return null;
        }
        
        PlayerStateMachine.Instance.stuck = false;

        if (stopsGravity) PlayerStateMachine.Instance.Rb.gravityScale = gS;
        
        if(PlayerStateMachine.Instance.DivePressed) PlayerStateMachine.Instance.ForceUpdateState();
        
        if(allowedPassage!=-1 && _passageCounter >= allowedPassage) sprRend.enabled = false;
        
    }

    

    private void Reset()
    {
        sprRend.enabled = true;
        _passageCounter = 0;
        PlayerStateMachine.Instance.Dies -= Reset;
    }
}
