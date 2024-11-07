using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class TotalDeathCounter : MonoBehaviour, IDataPersistance
{
    public static TotalDeathCounter Instance { get; private set; }

    public ulong TotalNumberOfDeath;
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
        

        UpdateDeathCounter();
    }

    public void saveData(GameData data)
    {
        ulong currentTotal = 0;
        foreach (ulong deathCount in data.LevelTotalDeathCount.Values)
        {
            currentTotal += deathCount;
        }
        
        data.TotalDeathCount = currentTotal; // Set, not add to, the total death count
        TotalNumberOfDeath = currentTotal;   // Update the displayed value
        
        UpdateDeathCounter();
    }
}
