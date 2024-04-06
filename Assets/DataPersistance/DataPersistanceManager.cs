using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
            Debug.LogError("Found more then one DataPersistanceManager");
        }
        Instance = this;
    }

    private void Start(){
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, FileName, useEncryption);
        this.dataPersistanceObjects = FindAllDataPersistanceObjects();
        LoadGame();
    }

    private void Update(){

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
            Debug.Log("No data found, Initializing Default");
            NewGame();
        }
        //Push data to other scripts
        foreach(IDataPersistance dataPersistanceObj in dataPersistanceObjects){
            dataPersistanceObj.loadData(_gameData);
        }
    }

    public void SaveGame(){
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
}
