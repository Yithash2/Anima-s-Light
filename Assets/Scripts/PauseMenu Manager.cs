using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private EventSystem _eventSystem;

    [Header("Pause Panel")]
    [SerializeField] private Animator _pausePanelAnimator;
    [SerializeField] private GameObject _pauseFirstSelected;
    [SerializeField] private AnimationClip _pausePanelClosing;
    [SerializeField] private AnimationClip _pausePanelOppening;
    [SerializeField] private AnimationClip _pausePanelClosed;
    [SerializeField] private AnimationClip _pausePanelOpen;
    [SerializeField] private List<MonoBehaviour> _playerControls;

    void Start()
    {
        
    }

    // Update is called once per frame
     void Update()
    {
        HanglePausePanelClosingOpening();

    }

    private void HanglePausePanelClosingOpening(){
        if(IsAnimating(_pausePanelClosed.name)){
            Time.timeScale = 1f;
            if(Input.GetKeyDown(KeyCode.Escape)){

                foreach(MonoBehaviour o in _playerControls){
                    o.enabled = false;
                }

                _eventSystem.SetSelectedGameObject(_pauseFirstSelected);
                _pausePanelAnimator.Play(_pausePanelOppening.name);
            
            } 
        }else if(IsAnimating(_pausePanelOpen.name)){
            Time.timeScale = 0f;
            if(Input.GetKeyDown(KeyCode.Escape)){

                foreach(MonoBehaviour o in _playerControls){
                    o.enabled = true;
                }

                Time.timeScale = 1f;
                _pausePanelAnimator.Play(_pausePanelClosing.name);
                _eventSystem.SetSelectedGameObject(null);
            }
            
        }
        
        
    }

    public void MenuButton(){
        DataPersistanceManager.Instance.SaveGame();
        Time.timeScale = 1f;
        SceneManager.LoadScene("LevelSelector _temp");
    }

    public void Quit(){
        Application.Quit();
    }
    

    private bool IsAnimating(string animationName){
        if(_pausePanelAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName)){
            return true;
        }else{
            return false;
        }
    }

    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
        DataPersistanceManager.Instance.RestartLevel();
    }
}
