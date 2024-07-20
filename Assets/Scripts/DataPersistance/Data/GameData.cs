using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string CurentCheckpoint;

    public SerializeDictionary<String, bool> checkpoints;

    public GameData(){

        this.CurentCheckpoint = "";
        this.checkpoints = new SerializeDictionary<string, bool>();

    }
}
