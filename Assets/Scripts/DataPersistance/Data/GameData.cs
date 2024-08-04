using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public SerializeDictionary<string, int> Levels; 
    public SerializeDictionary<string, int> checkpoints;

    public GameData(){

        this.Levels = new SerializeDictionary<string,int>();
        this.checkpoints = new SerializeDictionary<string,int>();


    }
}
