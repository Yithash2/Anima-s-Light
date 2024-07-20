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
    }

    private void OnEnable(){
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable(){
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        this.dataPersistanceObjects = FindAllDataPersistanceObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene current){
        SaveGame();
    }


    public void DeleteData(){
        dataHandler.Reset();
    }

    public void NewGame(){
        this._gameData = new GameData();
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
        //TODO . Pass the data to other scripts
        foreach(IDataPersistance dataPersistanceObj in dataPersistanceObjects){
                    dataPersistanceObj.saveData(ref _gameData);
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
        return _gameData != null;
    }
}
