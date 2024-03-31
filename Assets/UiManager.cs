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
    [SerializeField] private GameObject _firstSelected;
    [SerializeField] private AnimationClip _pausePanelClosing;
    [SerializeField] private AnimationClip _pausePanelOppening;
    [SerializeField] private List<MonoBehaviour> _playerControls;
    private bool _pausePanelClosed = true;

    [Header("RestartButton")]
    [SerializeField] private DataPersistanceManager _dataPersistance; 
    [SerializeField] private Scene _currentScene;


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

            Time.timeScale = 0.2f;

            _eventSystem.SetSelectedGameObject(_firstSelected);
            _pausePanelAnimator.Play(_pausePanelOppening.name);
            _pausePanelClosed = false;
            
        }else if(Input.GetKeyDown(KeyCode.Escape)){

            foreach(MonoBehaviour o in _playerControls){
                o.enabled = true;
            }

            Time.timeScale = 1;

            _pausePanelAnimator.Play(_pausePanelClosing.name);
            _pausePanelClosed = true;
            _eventSystem.SetSelectedGameObject(null);
        }
    }


    public void OptionButton(){
        //TODO : Options
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
