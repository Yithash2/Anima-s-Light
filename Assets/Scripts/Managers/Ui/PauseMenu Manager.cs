using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class PauseMenuManager : MonoBehaviour
{
    private EventSystem _eventSystem;

    [Header("Pause Panel")]
    [SerializeField] private Animator _pausePanelAnimator;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _pauseFirstSelected;
    [SerializeField] private AnimationClip _pausePanelClosing;
    [SerializeField] private AnimationClip _pausePanelOppening;
    [SerializeField] private AnimationClip _pausePanelClosed;
    [SerializeField] private AnimationClip _pausePanelOpen;


    [SerializeField] private GameObject _optionButton;
    [SerializeField] GameObject _optionFirstSelected;
    [SerializeField] GameObject _optionMenu;

    private bool _echapPressed = false;

    void Start()
    {
        _eventSystem = EventSystem.current;
    }

    // Update is called once per frame
     void Update()
    {
        HanglePausePanelClosingOpening();
        
    }

    public void Echap(InputAction.CallbackContext ctx){
        if(ctx.started){
            _echapPressed = true;
        }
    }

    private void HanglePausePanelClosingOpening(){
            if(_pauseMenu.activeInHierarchy){
                if(IsAnimating(_pausePanelClosed.name)){
                
                if(_echapPressed){
                    _echapPressed = false;
                    
                    _eventSystem.SetSelectedGameObject(_pauseFirstSelected);
                    _pausePanelAnimator.Play(_pausePanelOppening.name);
                
                } 
            }else if(IsAnimating(_pausePanelOpen.name)){
                Time.timeScale = 0f;

                if(_echapPressed){
                    _echapPressed = false;
                    Time.timeScale = 1f;
                    _pausePanelAnimator.Play(_pausePanelClosing.name);

                    _eventSystem.SetSelectedGameObject(null);
                }
                
            }
        }else if(_echapPressed && _optionMenu.activeInHierarchy){
            OptionMenu();
        }
        
        
    }

    public void Resume(){
        StartCoroutine(ClosePanel());
    }

    void OnEnable()
    {
        InputsManager.Instance.Echap.started += Echap;
        InputsManager.Instance.Echap.performed += Echap;
        InputsManager.Instance.Echap.canceled += Echap;
    }

    void OnDisable()
    {
        InputsManager.Instance.Echap.started -= Echap;
        InputsManager.Instance.Echap.performed -= Echap;
        InputsManager.Instance.Echap.canceled -= Echap;
    }


    IEnumerator ClosePanel(){
        yield return new WaitUntil(() => IsAnimating(_pausePanelOpen.name));

        Time.timeScale = 1f;
        _pausePanelAnimator.Play(_pausePanelClosing.name);

        PlayerStateMachine.Instance.JumpPressed = false;

        _eventSystem.SetSelectedGameObject(null);
    }

    public void OptionMenu(){
        if(_optionMenu.activeInHierarchy){
            _optionMenu.SetActive(false);
            _pauseMenu.SetActive(true);
            _pausePanelAnimator.Play("Open");
            _eventSystem.SetSelectedGameObject(_optionButton);
        }else{
            _optionMenu.SetActive(true);
            _pauseMenu.SetActive(false);

        }

    }

    public void MenuButton(){
        Time.timeScale = 1f;
        StartCoroutine(ClosePanel());
        SceneTransition.Instance.TransitionScene("MainMenu");
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
        Time.timeScale = 1f;
        DataPersistanceManager.Instance.RestartLevel();
        StartCoroutine(ClosePanel());
        SceneTransition.Instance.TransitionScene(SceneManager.GetActiveScene().name);
    }
}
