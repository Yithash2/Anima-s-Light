using Newtonsoft.Json;
using UnityEngine;

public class checkpoint : MonoBehaviour, IDataPersistance
{
    public PlayerData PlayerData;
    public bool IsPassableMultipleTimes;
    public string id;
    [ContextMenu("create new id")]
    void CreateNewId(){
        id = System.Guid.NewGuid().ToString();
    }
    public Transform Spawnpoint;
    public string PlayerTag;
    private bool isActivated = false;


    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag(PlayerTag) && !isActivated){
            Spawnpoint.position = transform.position;
            isActivated = true;
            PlayerData.CurrentCheckpointId = id;
        }else if(collider.CompareTag(PlayerTag) && IsPassableMultipleTimes){
            Spawnpoint.position = transform.position;
            PlayerData.CurrentCheckpointId = id;
        }
    }

    public void loadData(GameData data){
            
        data.checkpoints.TryGetValue(id, out isActivated);
            
    }

    public void saveData(ref GameData data){

        if(data.checkpoints.ContainsKey(id)){
            data.checkpoints.Remove(id);
        }
        data.checkpoints.Add(id, isActivated);
    }
}
