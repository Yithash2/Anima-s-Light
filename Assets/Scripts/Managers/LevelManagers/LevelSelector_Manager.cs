using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System;
using Levels;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;


public class LevelSelectorManager : MonoBehaviour, IDataPersistance
{
    public GameObject LevelInf;

    public Action<string> OnClearCurrentLevel;

    public static LevelSelectorManager Instance {get; private set;} 
    void Awake(){
        if(Instance != null){
            Debug.Log("Found more then one LevelSelectorManager, and destoyed it");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }  

    public void ReturnToMainMenu(){
        SceneManager.LoadScene("MainMenu");
    }

    public void loadData(GameData data){

        
    }

    public void saveData( GameData data){

    }

    public void LoadLevel(LevelScriptableObj Level){

        DataPersistanceManager.Instance.RestartLvlIfNotcurrent(Level.Name);

        OnClearCurrentLevel.Invoke(Level.Name);

        DataPersistanceManager.Instance.SaveGame();

        Instantiate(LevelInf);

        LevelInfo.Instance.Level = Level ;

        SceneTransition.Instance.TransitionScene(Level.SceneName);
    }


}

