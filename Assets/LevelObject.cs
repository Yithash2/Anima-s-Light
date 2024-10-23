using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Xml;

public class LevelObject : MonoBehaviour, IDataPersistance
{
    [SerializeField] TextMeshProUGUI _deathCounterText;
    [SerializeField] TextMeshProUGUI _bestTimeText;
    [SerializeField] ulong _deathCount;
    [SerializeField] float _bestTime;

    [SerializeField] string _name;
    
    void UpdateTimerText(float CurrentTime, TextMeshProUGUI Text){
        float hours = Mathf.FloorToInt(CurrentTime / 3600F);
        float minutes = Mathf.FloorToInt((CurrentTime - hours * 3600F) / 60F);
        float secondes = Mathf.FloorToInt(CurrentTime - hours * 3600F - minutes * 60F);
        float millisecondes = Mathf.Floor((CurrentTime - Mathf.Floor(CurrentTime)) * 1000F);

        string tempsFormatte = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", hours, minutes, secondes, millisecondes);

        Text.text = "Time : " + tempsFormatte;
    }

    public void loadData(GameData data){
        try{
            _deathCounterText.text = 'X' + data.LevelTotalDeathCount[_name].ToString();
            _deathCount = data.LevelTotalDeathCount[_name];
            UpdateTimerText(data.LevelBestTime[_name], _bestTimeText);
            _bestTime = data.LevelBestTime[_name];
        }catch{

        }
        
            
    }

    public void saveData(GameData data){

    }
}
