using Levels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour, IDataPersistance
{
    public static LevelInfo Instance {get; private set;}

    public LevelScriptableObj Level;

    public ulong TotalNbrDeath;
    public float BestTime;

    public string[] UnlockLevel;

    public bool SetLevelDeath = true;

    public bool LevelFinished;
    void Awake()
    {
        if(Instance != null && Instance != this){
            Debug.Log("Found more then one LevelInfo, and destoyed it");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }


    public IEnumerator SelfDestroy(){
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }

    public void loadData(GameData data)
    {
        Debug.Log(data.currentLevelDeathCount);
        Debug.Log(Level.Name);
        if(data.currentLevelDeathCount.ContainsKey(Level.Name))
        TotalNbrDeath = data.currentLevelDeathCount[Level.Name];
        else
        {
            TotalNbrDeath = 0;
            data.currentLevelDeathCount.Add(Level.Name, TotalNbrDeath);
        }
    }

    public void saveData(GameData data)
    {

        if (LevelFinished)
        {
            data.LevelBestTime[Level.Name] = BestTime;
            data.Levels[Level.Name] = 2;
            data.LevelTotalDeathCount[Level.Name] += TotalNbrDeath;
            
            foreach(string lvl in UnlockLevel)
            {
                data.Levels[lvl] = 0;
            }

            StartCoroutine(SelfDestroy());
        }
        else
        {
            data.currentLevelDeathCount.Clear();
            data.currentLevelDeathCount.Add(Level.Name, TotalNbrDeath);
        }
    }


}
