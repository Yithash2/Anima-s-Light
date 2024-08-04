using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{

    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private DataPersistanceManager _dataPersistance; 
    

    [Header("Option Menu")]
    [SerializeField] private GameObject _optionMenuButton;
    [SerializeField] private GameObject _optionMenu;
    [SerializeField] private GameObject _optionFirstSelected;
    private bool _optionMenuOpen = false;

    [Header("Main Menu")]
    [SerializeField] private GameObject _mainMenu;

    [Header("Play Menu")]
    [SerializeField] private GameObject _playMenuButton;
    [SerializeField] private GameObject _playMenu;
    [SerializeField] private GameObject _playFirstSelected;
    [SerializeField] private Button _continueButton;
    private bool _playMenuOpen = false;
    [Space(2)]

    private Camera[] cameras;
    


    // Start is called before the first frame update
    void Awake()
    {
    }

    void Start(){

        if(!DataPersistanceManager.Instance.HasGameData()){
            _continueButton.interactable = false;
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

    public void PlayButton(){
        if(_playMenuOpen){
            _playMenuOpen = false;
            _mainMenu.SetActive(true);
            _playMenu.SetActive(false);
            _eventSystem.SetSelectedGameObject(_playMenuButton);
        }else{
            _playMenuOpen = true;
            _mainMenu.SetActive(false);
            _playMenu.SetActive(true);
            _eventSystem.SetSelectedGameObject(_playFirstSelected);
        }
    }

    public void ContinueButton(){
        SceneManager.LoadScene("LevelSelector _temp");
    }

    public void NewGameButton(){
        DataPersistanceManager.Instance.NewGame();
        SceneManager.LoadScene("LevelSelector _temp");
    }

    public void MenuPauseButton(){
        _dataPersistance.SaveGame();
        SceneManager.LoadScene("LevelMenu");
    }

    public void QuitButton(){
        Application.Quit();
    }

    public void MenuButton(){
        //TODO : main menu
    }

}
