using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System;


public class LevelSelectorManager : MonoBehaviour, IDataPersistance
{
    public Level[] levels;
    public GameObject LevelInf;

    public GameObject WinScreen_temp; 

    public static LevelSelectorManager Instance {get; private set;} 
    void Awake(){
        if(Instance != null){
            Debug.Log("Found more then one LevelSelectorManager, and destoyed it");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }  
    void Start()
    {
        try{
            List<string> lvs = LevelInfo.Instance.UnlockLevel.ToList();
            Level lv = LevelInfo.Instance.level;

            bool hasWon = true;

            if(lvs[0] != "WIN") hasWon = false;
            
            for(int i = 0; i < levels.Length; i++){
                if(lv.LevelName == levels[i].LevelName){
                    levels[i].levelIndex = lv.levelIndex;
                    if(LevelInfo.Instance.SetLevelDeath){
                        levels[i].TotalDeathCount = lv.TotalDeathCount;
                    }

                    levels[i].BestTime = lv.BestTime;
                }

                if((lvs.Count != 0) && (lvs[0] == levels[i].LevelName)){
                    if(levels[i].levelIndex == -1) levels[i].levelIndex = 0;
                    lvs.RemoveAt(0);
                }

                if(levels[i].levelIndex < 2) hasWon = false;

            }

            DataPersistanceManager.Instance.SaveGame();
            DataPersistanceManager.Instance.LoadGame();

            LevelInfo.Instance.SelfDestroy();
        }catch{

        }

        foreach(Level level in levels){
            if(level.levelIndex == -1){
                level.button.interactable = false;
            }
        }
    }

    public void CONTINUE_temp(){
        WinScreen_temp.SetActive(false);
    }

    public void ReturnToMainMenu(){
        SceneManager.LoadScene("MainMenu");
    }

    public void loadData(GameData data){
        try{
            for(int i = 0; i < levels.Length; i++){
                levels[i].levelIndex = data.Levels[levels[i].LevelName];
                levels[i].TotalDeathCount = data.LevelTotalDeathCount[levels[i].LevelName];
                levels[i].BestTime = data.LevelBestTime[levels[i].LevelName];
            }

        }catch{
            
        }
        
    }

    public void saveData( GameData data){
        foreach(Level lv in levels){
            if(data.Levels.ContainsKey(lv.LevelName)){
                data.Levels.Remove(lv.LevelName);
                data.LevelTotalDeathCount.Remove(lv.LevelName);
                data.LevelBestTime.Remove(lv.LevelName);
            }
            data.Levels.Add(lv.LevelName, lv.levelIndex);
            data.LevelTotalDeathCount.Add(lv.LevelName, lv.TotalDeathCount);
            data.LevelBestTime.Add(lv.LevelName, lv.BestTime);

        }

        TotalDeathCounter.Instance.UpdateDeathCounter();
    }

    public void LoadLevel(string levelName){
        Level lv = new Level();
        foreach(Level level in levels){
            if(level.LevelName == levelName){
                lv = level;
                break;
            }
        }
       

        if(lv.levelIndex != 1 && lv.levelIndex != 3){

            DataPersistanceManager.Instance.RestartLevel();

            lv.levelIndex += 1;
            for(int i = 0; i < levels.Length; i++){
                if(lv.LevelName == levels[i].LevelName){
                    levels[i].levelIndex += 1;
                }else if(levels[i].levelIndex == 1 ||levels[i].levelIndex == 3){
                    levels[i].levelIndex -= 1;
                }
            }
        }

        DataPersistanceManager.Instance.SaveGame();
        
        Instantiate(LevelInf);

        LevelInfo.Instance.level = lv;

        SceneManager.LoadScene(lv.LevelSceneName);
    }

    /*void ClearCurrentLevelIndex(){
        for(int i = 0;i < levels.Length; i++){
            if(levels[i].levelIndex == 1 || levels[i].levelIndex == 3){
                levels[i].levelIndex -= 1;
            }
        }
    }*/


}

