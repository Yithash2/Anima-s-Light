using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq; 


[System.Serializable]
public struct Level{
    public string LevelName;
    public string LevelSceneName; 
    public Button button;
    public int levelIndex;
    /*
    Case -1 : Le niveau est pas débloquer
    Case 0 : niveau débloquer
    case 1 : Le dernier niveau jouer et pas terminé
    case 2 : niveau gagné
    case 3 : niveau gagné et dernier niveau jouer
    */
    public ulong TotalDeathCount;

    public float BestTime; //In miliseconds
    
}
