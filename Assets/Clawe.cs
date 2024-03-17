using UnityEngine;

public class Claw : MonoBehaviour
{
    int _posIndex = 0;

    [SerializeField] PlayerDateScriptableObject NormalPlayer;
    [SerializeField] PlayerDateScriptableObject ClawedPlayer;
    [SerializeField] PlayerStateMachine PSM;
    SpriteRenderer _spriteRenderer;
    [SerializeField] Animator _animator;

    void Start(){
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(){
        if(_posIndex == 0){
            PSM.PlayerData = ClawedPlayer;
            _animator.Play("Taken");
            _posIndex = 1;
        }else{
            PSM.PlayerData = NormalPlayer;
            _animator.Play("Base");
            _posIndex = 0;
        }
    }
    
}
