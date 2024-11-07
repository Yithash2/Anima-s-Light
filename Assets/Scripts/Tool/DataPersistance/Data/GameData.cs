using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public SerializeDictionary<string, int> Levels; 
    public SerializeDictionary<string, int> checkpoints;
    public SerializeDictionary<string, ulong> currentLevelDeathCount;
    public SerializeDictionary<string, ulong> LevelTotalDeathCount;
    public ulong TotalDeathCount;

    public SerializeDictionary<string, float> currentTimer;
    public SerializeDictionary<string, float> LevelBestTime;

    public GameData(){

        this.Levels = new SerializeDictionary<string,int>();
        this.checkpoints = new SerializeDictionary<string,int>();

        this.currentLevelDeathCount = new SerializeDictionary<string,ulong>();
        this.LevelTotalDeathCount = new SerializeDictionary<string,ulong>();
        this.TotalDeathCount = 0;

        this.currentTimer = new SerializeDictionary<string, float>();
        this.LevelBestTime = new SerializeDictionary<string, float>();


    }
}
