using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelObject : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _deathCounterText;
    [SerializeField] TextMeshProUGUI _bestTimeText;

    [SerializeField] string _name;

    // Update is called once per frame
    void Start()
    {
        LevelSelectorManager _lvls = LevelSelectorManager.Instance;

        foreach(Level l in _lvls.levels){
            if(l.LevelName == _name){
                _deathCounterText.text = "X" + l.TotalDeathCount.ToString();
                UpdateTimerText(l.BestTime ,_bestTimeText);
                break;
            }
        }

        
        
        
    }
    
    void UpdateTimerText(float CurrentTime, TextMeshProUGUI Text){
        float hours = Mathf.FloorToInt(CurrentTime / 3600F);
        float minutes = Mathf.FloorToInt((CurrentTime - hours * 3600F) / 60F);
        float secondes = Mathf.FloorToInt(CurrentTime - hours * 3600F - minutes * 60F);
        float millisecondes = Mathf.Floor((CurrentTime - Mathf.Floor(CurrentTime)) * 1000F);

        string tempsFormatte = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", hours, minutes, secondes, millisecondes);

        Text.text = "Time : " + tempsFormatte;
    }
}
