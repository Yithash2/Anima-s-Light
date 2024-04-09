using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{

    public static UiManager Instance {get; private set;}

    [SerializeField] private EventSystem _eventSystem;

    [Header("Pause Panel")]
    [SerializeField] private Animator _pausePanelAnimator;
    [SerializeField] private GameObject _pauseFirstSelected;
    [SerializeField] private AnimationClip _pausePanelClosing;
    [SerializeField] private AnimationClip _pausePanelOppening;
    [SerializeField] private List<MonoBehaviour> _playerControls;
    private bool _pausePanelClosed = true;

    [Header("RestartButton")]
    [SerializeField] private DataPersistanceManager _dataPersistance; 
    [SerializeField] private Scene _currentScene;
    

    [Header("Option Menu")]
    [SerializeField] private GameObject _optionMenuButton;
    [SerializeField] private GameObject _optionMenu;
    [SerializeField] private GameObject _optionFirstSelected;
    private bool _optionMenuOpen = false;

    [Header("Main Menu")]
    [SerializeField] private GameObject _mainMenu;

    private Camera[] cameras;
    


    // Start is called before the first frame update
    void Awake()
    {
        if(Instance != null){
            Debug.LogError("Found more then one DataPersistanceManager");
        }
        Instance = this;
    }

    void Start(){
        _currentScene = SceneManager.GetActiveScene();

        cameras = FindObjectsOfType<Camera>();
        SetCameraAspect(cameras);
    }

    // Update is called once per frame
    void Update()
    {
        HanglePausePanelClosingOpening();

    }

    private void HanglePausePanelClosingOpening(){
        if(Input.GetKeyDown(KeyCode.Escape) && _pausePanelClosed){

            foreach(MonoBehaviour o in _playerControls){
                o.enabled = false;
            }

            _eventSystem.SetSelectedGameObject(_pauseFirstSelected);
            _pausePanelAnimator.Play(_pausePanelOppening.name);
            _pausePanelClosed = false;
            
        }else if(Input.GetKeyDown(KeyCode.Escape)){

            foreach(MonoBehaviour o in _playerControls){
                o.enabled = true;
            }


            _pausePanelAnimator.Play(_pausePanelClosing.name);
            _pausePanelClosed = true;
            _eventSystem.SetSelectedGameObject(null);
        }
    }

    private void SetCameraAspect(Camera[] cams){
        foreach (Camera cam in cams){
            cam.aspect = 16/9;
        }
    }


    public void OptionButton(){  
        if(_optionMenuOpen){
            _optionMenuOpen = false;
            _mainMenu.SetActive(true);
            _optionMenu.SetActive(false);
            _eventSystem.SetSelectedGameObject(_optionMenuButton);
        }else{
            _optionMenuOpen = true;
            _mainMenu.SetActive(false);
            _optionMenu.SetActive(true);
            _eventSystem.SetSelectedGameObject(_optionFirstSelected);
        }
        
        
    }

    public void QuitButton(){
        Application.Quit();
    }

    public void MenuButton(){
        //TODO : main menu
    }

    public void Restart(){
        _dataPersistance.DeleteData();
        SceneManager.LoadScene(_currentScene.buildIndex, LoadSceneMode.Single);
    }
}
