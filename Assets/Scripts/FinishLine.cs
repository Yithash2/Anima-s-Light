using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    public string[] unlockableLevelID;
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        LevelInfo.Instance.UnlockLevel = unlockableLevelID;
        LevelInfo.Instance.level.levelIndex = 2;

        DataPersistanceManager.Instance.RestartLevel();

        SceneManager.LoadScene(1);
        
    }


}
