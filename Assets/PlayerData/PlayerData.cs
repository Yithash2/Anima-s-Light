using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour, IDataPersistance
{
    public string CurrentCheckpointId;
    private Transform player;
    [SerializeField] private Transform respawnPoint;

    public void loadData(GameData data){
        this.CurrentCheckpointId = data.CurentCheckpoint;
    }

    public void saveData(ref GameData data){
        data.CurentCheckpoint = this.CurrentCheckpointId;
    }

    public void Start(){
        this.player = GetComponent<Transform>();

        checkpoint[] checkpoints = FindObjectsOfType<checkpoint>();

        if(CurrentCheckpointId != null){
            foreach(checkpoint check in checkpoints){
                if(check.id == CurrentCheckpointId){
                    player.position = check.gameObject.transform.position;
                    respawnPoint.position = check.gameObject.transform.position;
                }
            }
        }

    }

    
}
