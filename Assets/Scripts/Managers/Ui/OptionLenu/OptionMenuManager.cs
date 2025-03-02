using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;




public class OptionMenuManager : MonoBehaviour
{

    Tabulation _currentTab;
    public Tabulation CurrentTab {get{return _currentTab;}}
    [SerializeField] AudioMixer audioMixer;

    [SerializeField] Tabulation[] tabs;

    [Header("Audio tab")]
    [SerializeField] Slider _masterVolumeSlider;
    [SerializeField] Slider _musicVolumeSlider;
    [SerializeField] Slider _SFXVolumeSlider;
    EventSystem _eventSystem;

    [Header("Controls tab")]
    [SerializeField] GameObject _controlChoisePanel;
    [SerializeField] GameObject _keyboardControlButton;
    [SerializeField] GameObject _keyboardControlPanel;
    [SerializeField] GameObject _keyboardControlFirstSelected;
    [SerializeField] GameObject _controllerControlPanel;
    [SerializeField] GameObject _controllerControlButton;
    [SerializeField] GameObject _controllerControlFirstSelected;

    [Header("Graphic tab")]
    [SerializeField] Toggle _windowedToggle;
    [SerializeField] Toggle _showSpeedrunTimeToggle;
    [SerializeField] GameObject _speedrunTimeGO;
    [SerializeField] Toggle _showDeathCountToggle;
    [SerializeField] GameObject _deathCountGO;
    [SerializeField] Toggle _hueShift;
    
    [SerializeField] Button _backButton;


    public void Awake(){
        _eventSystem = EventSystem.current;

        SearchForTab(tabs[0].TabName);

        UpdateVolumes();

        if(Screen.fullScreen) _windowedToggle.isOn = false;
        else _windowedToggle.isOn = true;

        if(PlayerPrefs.GetInt("Speedrunner") == 1)_showSpeedrunTimeToggle.isOn = true;
        else _showSpeedrunTimeToggle.isOn = false;

        if(PlayerPrefs.GetInt("Winner") == 1) _showDeathCountToggle.isOn = true;
        else _showDeathCountToggle.isOn = false;

        if (PlayerPrefs.GetInt("DoHueShift") == 1) _hueShift.isOn = true;
        else _hueShift.isOn = false;



    }
    public void KeyboardControls(){
        if(!_keyboardControlPanel.activeInHierarchy){
            _controlChoisePanel.SetActive(false);
            _keyboardControlPanel.SetActive(true);
            _eventSystem.SetSelectedGameObject(_keyboardControlFirstSelected);
        }else{
            _controlChoisePanel.SetActive(true);
            _keyboardControlPanel.SetActive(false);
            _eventSystem.SetSelectedGameObject(_keyboardControlButton);
        }
    }

    public void SeizureToggle(bool toggle)
    {
        int i = toggle ? 1 : 0;
        PlayerPrefs.SetInt("DoHueShift", i);
    }

    public void ControllerControls(){
        if(!_controllerControlPanel.activeInHierarchy){
            _controlChoisePanel.SetActive(false);
            _controllerControlPanel.SetActive(true);
            _eventSystem.SetSelectedGameObject(_controllerControlFirstSelected);
        }else{
            _controlChoisePanel.SetActive(true);
            _controllerControlPanel.SetActive(false);
            _eventSystem.SetSelectedGameObject(_controllerControlButton);
        }
    }
    
    public void OpenTab(string TabName){  
        _controlChoisePanel.SetActive(true);
        _controllerControlPanel.SetActive(false);
        _keyboardControlPanel.SetActive(false); 
        SearchForTab(TabName);
    }


    public void UpdateVolumes(){
        _masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        _musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        _SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");

        MasterVolume(_masterVolumeSlider.value);
        SFXVolume(_SFXVolumeSlider.value);
        MusicVolume(_musicVolumeSlider.value);
    }

    public void MasterVolume(float value){
        audioMixer.SetFloat("Master Volume", Mathf.Log10(value)*20);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void SFXVolume(float value){
        audioMixer.SetFloat("SFX Volume", Mathf.Log10(value)*20);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    public void MusicVolume(float value){
        audioMixer.SetFloat("Music Volume", Mathf.Log10(value)*20);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }
    
    public void Back(){
        SearchForTab(tabs[0].TabName);

        UpdateVolumes();
    }

    public void FullScrrenToggle(bool toggle){
        Screen.fullScreen = !toggle;
    }

    public void SpeedrunTileToggle(bool toggle){
        if (toggle) {
            PlayerPrefs.SetInt("Speedrunner", 1);
            _speedrunTimeGO.SetActive(true);
        }else {
            PlayerPrefs.SetInt("Speedrunner", 0);
            _speedrunTimeGO.SetActive(false);
        }
    }

    public void ShowDeathCounterToggle(bool toggle){
        if (toggle) {
            PlayerPrefs.SetInt("Winner", 1);
            _deathCountGO.SetActive(true);
        }else{
            PlayerPrefs.SetInt("Winner", 0);
            _deathCountGO.SetActive(false);
        }

    }

    public void SearchForTab(string TabToFind){
        
        foreach(Tabulation tab in tabs ){
            if(tab.TabName != TabToFind){
                tab.TabGameObject.SetActive(false);
                tab.TabButton.interactable = true;
            }else{
                tab.TabGameObject.SetActive(true);
                tab.TabButton.interactable=false;
                _eventSystem.SetSelectedGameObject(tab.FirstSelected);
            }
        }
       
    }
}
