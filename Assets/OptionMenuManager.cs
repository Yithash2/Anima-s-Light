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

    [SerializeField] Slider _masterVolumeSlider;
    [SerializeField] Slider _musicVolumeSlider;
    [SerializeField] Slider _SFXVolumeSlider;
    EventSystem _eventSystem;

    [SerializeField] Button _backButton;

    public InputActionReference cancel { get; set; }

    public void Start(){
        _eventSystem = EventSystem.current;

        _currentTab = tabs[0];
        _currentTab.TabGameObject.SetActive(true);
        _eventSystem.SetSelectedGameObject(_currentTab.FirstSelected);
        _currentTab.TabButton.interactable = false;

        UpdateVolumes();
    }

    void OnEnable(){
        _currentTab = tabs[0];
        _currentTab.TabGameObject.SetActive(true);
        _eventSystem.SetSelectedGameObject(_currentTab.FirstSelected);
        _currentTab.TabButton.interactable = false;
    }

    public void OpenTab(string TabName){
        
        _currentTab.TabGameObject.SetActive(false);
        _currentTab.TabButton.interactable = true;
        
        _currentTab = SearchForTab(TabName);
        _currentTab.TabGameObject.SetActive(true);
        _eventSystem.SetSelectedGameObject(_currentTab.FirstSelected);
        _currentTab.TabButton.interactable = false;
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
        audioMixer.SetFloat("Master Volume", (Mathf.Log10(value)*20));
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
    

    public Tabulation SearchForTab(string TabToFind){

        Dictionary<string, Tabulation> tabsSearch = new Dictionary<string, Tabulation>();
        
        foreach(Tabulation tab in tabs ){
            tabsSearch.Add(tab.TabName, tab);
        }

        Tabulation _tabFound = new Tabulation();
        tabsSearch.TryGetValue(TabToFind,out _tabFound);
        return _tabFound;
        
    }
}
