using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathCounter : MonoBehaviour, IDataPersistance
{
    public ulong DeathCount;

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

    public void AddToDeathCount(){
        DeathCount += 1;

        LevelInfo.Instance.level.TotalDeathCount += 1;

        UpdateDeathCount();
    }

    void UpdateDeathCount(){
        DeathCounterText.text = DeathCount.ToString() + "x";
    }


    public void loadData(GameData data){
            if(data.currentLevelDeathCount != null){
                data.currentLevelDeathCount.TryGetValue(LevelInfo.Instance.level.LevelName, out DeathCount);
            }

            LevelInfo.Instance.level.TotalDeathCount += DeathCount;
            UpdateDeathCount();
    }

    public void saveData(GameData data){
        data.currentLevelDeathCount.Clear();
        data.currentLevelDeathCount = new SerializeDictionary<string, ulong>();
        data.currentLevelDeathCount.Add(LevelInfo.Instance.level.LevelName, DeathCount);
    }
}
