using UnityEngine;

[System.Serializable]
public class Claw : MonoBehaviour
{

    int _posIndex;

    [SerializeField] PlayerDateScriptableObject NormalPlayer;
    [SerializeField] PlayerDateScriptableObject ClawedPlayer;
    [SerializeField] PlayerStateMachine PSM;
    [SerializeField] Animator _animator;

    void Start(){
        _posIndex = 0;
        PSM.PlayerData = NormalPlayer;
    }

    void DoSomething(){

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

    void OnTriggerEnter2D(Collider2D collider){
        DoSomething();
    }

}

