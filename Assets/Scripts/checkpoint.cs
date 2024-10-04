using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;

public class checkpoint : MonoBehaviour, IDataPersistance
{
    public bool IsPassableMultipleTimes;
    public string id;
    [ContextMenu("create new id")]
    void CreateNewId(){
        id = System.Guid.NewGuid().ToString();
    }
    public Transform Spawnpoint;
    public string PlayerTag;
     public int isActivated = 0;

    private checkpoint[] checks;

    void Start(){
        checks = FindObjectsOfType<checkpoint>();

        if(isActivated == 2){
            Spawnpoint.SetParent(transform);
            Spawnpoint.position = transform.position;
            Transform Player = GameObject.Find("Player").transform;
            Player.position = transform.position;
        }
    }

    void UpdateCurrentCheckpoint(){
        foreach(checkpoint check in checks){
            if(check.isActivated == 2){
                check.isActivated = 1;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(isActivated != 2){
            if(collider.CompareTag(PlayerTag) && isActivated == 0){
                Spawnpoint.SetParent(transform);
                Spawnpoint.position = transform.position;
                UpdateCurrentCheckpoint();
                isActivated = 2;
            }else if(collider.CompareTag(PlayerTag) && IsPassableMultipleTimes){
                Spawnpoint.SetParent(transform);
            }

            DataPersistanceManager.Instance.SaveGame();
        }
    }

    public void loadData(GameData data){
        try{
            isActivated = data.checkpoints[id];
        }catch{
            
        }
             
    }

    public void saveData(GameData data){
        
        if(data.checkpoints.ContainsKey(id)){
            data.checkpoints.Remove(id);
        }
        data.checkpoints.Add(id, isActivated);
        
        
    }
}
