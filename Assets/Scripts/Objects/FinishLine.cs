using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    public string[] unlockableLevelID;
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        LevelInfo.Instance.UnlockLevel = unlockableLevelID;
        LevelInfo.Instance.LevelFinished = true;

        if(LevelInfo.Instance.BestTime > 0){
            if(SpeedrunClock.Instance.CurrentTime < LevelInfo.Instance.BestTime){
                LevelInfo.Instance.BestTime = SpeedrunClock.Instance.CurrentTime;
            }
        }else{
            LevelInfo.Instance.BestTime = SpeedrunClock.Instance.CurrentTime;
        }
        
        

        DataPersistanceManager.Instance.RestartLevel();

        DataPersistanceManager.Instance.SaveGame();

        SceneTransition.Instance.TransitionScene("NewLevelSelector Test");
        
    }


}
