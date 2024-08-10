using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class TotalDeathCounter : MonoBehaviour, IDataPersistance
{
    public static TotalDeathCounter Instance { get; private set; }

    public int TotalNumberOfDeath;
    public GameObject DeathCounterObject;
    public TextMeshProUGUI TotalNumberOfDeathText;

    void Awake(){
        if(Instance != null){
            Debug.Log("Found more then one TotalDeathCounter, and destoyed it");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }
    void Start()
    {
        if(PlayerPrefs.GetInt("Winner") == 0){
            DeathCounterObject.SetActive(false);
        }else{
            DeathCounterObject.SetActive(true);
        }

        UpdateDeathCounter();
    }

    public void UpdateDeathCounter(){
        TotalNumberOfDeathText.text = TotalNumberOfDeath.ToString() + "x";
    }

    public void loadData(GameData data){
        TotalNumberOfDeath = data.TotalDeathCount;  
    }

    public void saveData(GameData data){
        TotalNumberOfDeath = 0;
        

        if(DataPersistanceManager.Instance.HasPlayedALevel()){
            foreach(Level lvl in LevelSelectorManager.Instance.levels){    
                TotalNumberOfDeath +=  lvl.TotalDeathCount;
            }
            TotalNumberOfDeath += data.currentLevelDeathCount.First().Value;
        }

        data.TotalDeathCount = TotalNumberOfDeath;
    }
}
