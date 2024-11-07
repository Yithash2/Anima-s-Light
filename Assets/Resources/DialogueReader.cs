using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{

    [SerializeField]
    Line[] lines;

    int _lineIndex = -1;
    
    [ContextMenu("StartDialog")]
    public void StartDialog(){
        _lineIndex = 0;
        DialogueManager.Instance.UpdateDialog(lines[_lineIndex].text);
    }
    [ContextMenu("NextLine")]
    public void NextLine(){
        if(_lineIndex < lines.Length){
            _lineIndex ++;
            DialogueManager.Instance.UpdateDialog(lines[_lineIndex].text);
            }
        else EndDialog(); 
    }

    public void EndDialog(){

    }
}

[Serializable]
public struct Line{
    [TextArea]
    public string text;
    public float _lineTime;
}
