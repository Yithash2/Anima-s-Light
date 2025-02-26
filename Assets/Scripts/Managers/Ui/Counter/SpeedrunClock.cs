using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedrunClock : MonoBehaviour, IDataPersistance
{
    public static SpeedrunClock Instance { get; private set; }
    public float CurrentTime = 0;
    [SerializeField] GameObject _chronoMeter;
    [SerializeField] TextMeshProUGUI _chronoMeterText;
    public bool ChronoStopped = false;
    void Awake()
    {
        if(Instance != null){
            Debug.Log("We found another SpeedrunClock");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    void Start(){
        if(PlayerPrefs.GetInt("Speedrunner") == 0) _chronoMeter.SetActive(false);
        else _chronoMeter.SetActive(true);
    }

    void Update()
    {
        if(!ChronoStopped){
            UpdateTimer();
            UpdateTimerText();
        }
    }

    void UpdateTimer(){
        CurrentTime += Time.deltaTime;
    }

    void UpdateTimerText(){
        float hours = Mathf.FloorToInt(CurrentTime / 3600F);
        float minutes = Mathf.FloorToInt((CurrentTime - hours * 3600F) / 60F);
        float secondes = Mathf.FloorToInt(CurrentTime - hours * 3600F - minutes * 60F);
        float millisecondes = Mathf.Floor((CurrentTime - Mathf.Floor(CurrentTime)) * 1000F);

        string tempsFormatte = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", hours, minutes, secondes, millisecondes);

        _chronoMeterText.text = tempsFormatte;
    }

    public void loadData(GameData data){
        if(data.currentTimer != null){
            data.currentTimer.TryGetValue(LevelInfo.Instance.Level.Name, out CurrentTime );
        }
    }

    public void saveData(GameData data){
        data.currentTimer.Clear();
        data.currentTimer = new SerializeDictionary<string, float>
        {
            { LevelInfo.Instance.Level.Name, CurrentTime }
        };
    }
}
