using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Xml;
using System;
using Levels;
using SuperTiled2Unity;

public class LevelObject : MonoBehaviour, IDataPersistance
{
    [SerializeField] SpriteRenderer _spr;

    [SerializeField] TextMeshProUGUI _deathCounterText;
    [SerializeField] TextMeshProUGUI _bestTimeText;
    [SerializeField] ulong _deathCount;
    [SerializeField] float _bestTime;

    [SerializeField]private int _levelIndex;

    [field : SerializeField] public LevelScriptableObj Level { get; private set; }

    public string Name { get { return Level.Name; } }

    private void Start()
    {
        LevelSelectorManager.Instance.OnClearCurrentLevel += ClearCurentIndex;

    }

    public void ChangeLevelIndex(int new_LevelIndex)
    {
        _levelIndex = new_LevelIndex;
        ChangeSprite(new_LevelIndex);
    }

    public void ChangeSprite(int new_LevelIndex)
    {
        switch (new_LevelIndex)
        {
            case -1:
                _spr.sprite = Level.SpriteDesabledSprite;
                break;
            case 0:
                _spr.sprite = Level.ActivateSprite;
                break;
            case 1:
                _spr.sprite = Level.ActivateSprite;
                break;
            default :
                _spr.sprite = Level.WonSprite;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_levelIndex == -1) return;

        LevelSelectorManager.Instance.LoadLevel(Level);
    }

    void UpdateTimerText(float CurrentTime, TextMeshProUGUI Text){
        string tempsFormatte;

        if(CurrentTime == 0){
            tempsFormatte = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", 0, 0, 0, 0);

            Text.text = "Time : " + tempsFormatte;
            return;
        }

        float hours = Mathf.FloorToInt(CurrentTime / 3600F);
        float minutes = Mathf.FloorToInt((CurrentTime - hours * 3600F) / 60F);
        float secondes = Mathf.FloorToInt(CurrentTime - hours * 3600F - minutes * 60F);
        float millisecondes = Mathf.Floor((CurrentTime - Mathf.Floor(CurrentTime)) * 1000F);

        tempsFormatte = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", hours, minutes, secondes, millisecondes);

        Text.text = "Time : " + tempsFormatte;
    }

    public void loadData(GameData data){
        if (!data.Levels.ContainsKey(Name))
        {
            DataPersistanceManager.Instance.SaveGame();
        }
        else
        {
            //_deathCounterText.text = 'X' + data.LevelTotalDeathCount[Name].ToString();
            _deathCount = data.LevelTotalDeathCount[Name];
            //UpdateTimerText(data.LevelBestTime[Name], _bestTimeText);
            _bestTime = data.LevelBestTime[Name];
            ChangeLevelIndex( data.Levels[Name]);
        }

            
        
            
    }

    public void saveData(GameData data){
        if (data.Levels.ContainsKey(Name))
        {
            Debug.Log($"{_levelIndex} : {data.Levels[Name]}" );
            data.Levels[Name] = _levelIndex;
            data.LevelBestTime[Name] = _bestTime;
        }
        else
        {
            data.Levels.Add(Name, _levelIndex);
            data.LevelBestTime.Add(Name, _bestTime);
            data.LevelTotalDeathCount.Add(Name, 0);
        }

        
    }

    void ClearCurentIndex(string SearchedName)
    {
        if(Name == SearchedName)
        {
            switch (_levelIndex)
            {
                case 1:
                case 3:
                    break;
                default:
                    _levelIndex++;
                    break;

            }

            return;
        }

        switch(_levelIndex){
            case 1:
                ChangeLevelIndex(0);
                break;
            case 3:
                ChangeLevelIndex(2);
                break;
        }
    }

    private void OnValidate()
    {
        _spr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        LevelSelectorManager.Instance.OnClearCurrentLevel -= ClearCurentIndex;
    }
}
