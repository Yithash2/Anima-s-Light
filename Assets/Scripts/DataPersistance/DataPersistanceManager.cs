using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string FileName;
    [SerializeField] private bool useEncryption;

    public static DataPersistanceManager Instance {get; private set;}

    private GameData _gameData;
    public bool _levelRestart {get; private set;}
    private List<IDataPersistance> dataPersistanceObjects;
    private FileDataHandler dataHandler;
    private void Awake(){

        if(Instance != null){
            Debug.Log("Found more then one DataPersistanceManager, and destoyed it");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, FileName, useEncryption);

        _levelRestart = false;
    }

    public void RestartLevel(){
        _gameData.checkpoints.Clear();
        _gameData.currentTimer.Clear();

        SetTotalDeathCount();

        dataHandler.Save(_gameData);
    }

    public void SetTotalDeathCount(){
        if(HasPlayedALevel()){
            _gameData.LevelTotalDeathCount[_gameData.currentLevelDeathCount.First().Key] += _gameData.currentLevelDeathCount.First().Value;
        }
        _gameData.currentLevelDeathCount.Clear();
    }

    public bool HasPlayedALevel(){
        foreach(KeyValuePair<string, int> pair in _gameData.Levels){
            if(pair.Value == 1 || pair.Value == 3){
                return true;
            }
        }

        return false;
    }

    private void OnEnable(){
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable(){
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        
        this.dataPersistanceObjects = FindAllDataPersistanceObjects();
        LoadGame();
        Time.timeScale = 1f;
    }

    public void DeleteData(){
        dataHandler.Reset();
    }

    public void NewGame(){
        this._gameData = new GameData();
        SaveGame();
    }

    public void LoadGame(){
        this._gameData = dataHandler.Load();
        

        //TODO . LOAD ANY SAVE DATA FROM A FILE
        if(this._gameData == null){
            Debug.Log("No data found. A new game needs to be started before data can be loaded");
            return;
        }
        //Push data to other scripts
        foreach(IDataPersistance dataPersistanceObj in dataPersistanceObjects){
            dataPersistanceObj.loadData(_gameData);
        }
    }

    public void SaveGame(){
        if(this._gameData == null){
            Debug.LogWarning("No data was found. A new Game needs to be started before data can be saved");
            return;
        }
        
        foreach(IDataPersistance dataPersistanceObj in dataPersistanceObjects){
                    dataPersistanceObj.saveData(_gameData);
                }
        //save that data in a file
        dataHandler.Save(_gameData);
    }

    private void OnApplicationQuit(){
        SaveGame();
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects(){
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();

        return new List<IDataPersistance>(dataPersistanceObjects);
    }

    public bool HasGameData(){
        return dataHandler.GameDataExist();
    }
}
