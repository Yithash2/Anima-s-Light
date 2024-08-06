using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq; 


public class LevelSelector_Manager : MonoBehaviour, IDataPersistance
{
    public Level[] levels;
    public GameObject LevelInf;    
    void Start()
    {
        try{
            List<string> lvs = LevelInfo.Instance.UnlockLevel.ToList();
            Level lv = LevelInfo.Instance.level;

            for(int i = 0; i < levels.Length; i++){
                if(lv.LevelName == levels[i].LevelName){
                    levels[i].levelIndex = lv.levelIndex;
                }

                if((lvs.Count != 0) && (lvs[0] == levels[i].LevelName)){
                    levels[i].levelIndex = 0;
                    lvs.RemoveAt(0);
                }
            }

            LevelInfo.Instance.SelfDestroy();
        }catch{

        }

        DataPersistanceManager.Instance.SaveGame();

        foreach(Level level in levels){
            if(level.levelIndex == -1){
                level.button.interactable = false;
            }
        }
    }

    public void ReturnToMainMenu(){
        SceneManager.LoadScene("MainMenu");
    }

    public void loadData(GameData data){
        try{
            for(int i = 0; i < levels.Length; i++){
                levels[i].levelIndex = data.Levels[levels[i].LevelName];
            }

        }catch{
            
        }
        
    }

    public void saveData( GameData data){
        foreach(Level lv in levels){
            if(data.Levels.ContainsKey(lv.LevelName)){
                data.Levels.Remove(lv.LevelName);
            }
            data.Levels.Add(lv.LevelName, lv.levelIndex);
        }
    }

    public void LoadLevel(string levelName){
        Level lv = new Level();
        foreach(Level level in levels){
            if(level.LevelName == levelName){
                lv = level;
                break;
            }
        }
        Instantiate(LevelInf);

        ClearCurrentLevelIndex();

        if(lv.levelIndex != 1 && lv.levelIndex != 3){

            DataPersistanceManager.Instance.RestartLevel();

            lv.levelIndex += 1;
            for(int i = 0; i < levels.Length; i++){
                if(lv.LevelName == levels[i].LevelName){
                    levels[i].levelIndex += 1;
                }
            }
        }

        LevelInfo.Instance.level = lv;

        DataPersistanceManager.Instance.SaveGame();

        SceneManager.LoadScene(lv.LevelSceneName);
    }

    void ClearCurrentLevelIndex(){
        for(int i = 0;i < levels.Length; i++){
            if(levels[i].levelIndex == 1 || levels[i].levelIndex == 3){
                levels[i].levelIndex -= 1;
            }
        }
    }


}

