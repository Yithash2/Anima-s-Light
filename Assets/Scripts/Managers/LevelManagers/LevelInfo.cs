using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    public static LevelInfo Instance {get; private set;}

    public Level level;

    public string[] UnlockLevel;

    public bool SetLevelDeath = true;
    void Awake()
    {
        if(Instance != null){
            Debug.Log("Found more then one LevelInfo, and destoyed it");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void SelfDestroy(){
        Destroy(this.gameObject);
    }


}
