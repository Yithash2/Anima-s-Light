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
        LevelInfo.Instance.level.levelIndex = 2;

        if(LevelInfo.Instance.level.BestTime > 0){
            if(SpeedrunClock.Instance.CurrentTime < LevelInfo.Instance.level.BestTime){
                LevelInfo.Instance.level.BestTime = SpeedrunClock.Instance.CurrentTime;
            }
        }else{
            LevelInfo.Instance.level.BestTime = SpeedrunClock.Instance.CurrentTime;
        }
        
        

        DataPersistanceManager.Instance.RestartLevel();

        SceneManager.LoadScene(1);
        
    }


}
