using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathCounter : MonoBehaviour, IDataPersistance
{

    [SerializeField] GameObject DeathCounterUI;
    [SerializeField] TextMeshProUGUI DeathCounterText;

    public static DeathCounter Instance { get; private set; }
    public void Awake(){
        if(Instance != null){
            Debug.Log("Found more then one DeathCounter, and destoyed it");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    public void Start(){
        if(PlayerPrefs.GetInt("Winner") == 0){
            DeathCounterUI.SetActive(false);
        }else{
            DeathCounterUI.SetActive(true);
        }
    }

    public void AddToDeathCount() {
        LevelInfo.Instance.TotalNbrDeath += 1;

        UpdateDeathCount();
    }

    void UpdateDeathCount(){
        DeathCounterText.text = LevelInfo.Instance.TotalNbrDeath.ToString() + "x";
    }


    public void loadData(GameData data){
        UpdateDeathCount();
    }

    public void saveData(GameData data){
    }
}
