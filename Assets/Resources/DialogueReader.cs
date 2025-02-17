using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dialogue : MonoBehaviour
{

    [SerializeField]
    Dialog[] _dialogues;
    public int CurrentDialogue;
    public bool IsAutomaticDialogue { get { return _dialogues[CurrentDialogue].AutomaticNextLine; } }

    [ContextMenu("NextLine")]
    public void NextLine(){
        if(DialogueManager.Instance.CurrentDialogue == null || DialogueManager.Instance.CurrentDialogue == this){
            DialogueManager.Instance.CurrentDialogue = this;
            if(_dialogues[CurrentDialogue].CurrentLineTime == -1){
                EndDialog();
                return;
            }else{
                DialogueManager.Instance.UpdateDialog(_dialogues[CurrentDialogue].CurrentLine, _dialogues[CurrentDialogue].CurrentLineTime, _dialogues[CurrentDialogue].CurrentIcon);
                _dialogues[CurrentDialogue].LineIndex ++;
                DialogueManager.Instance.ShowDialogBoc(true);
            }
        }
    }

    public void EndDialog(){
        DialogueManager.Instance.CurrentDialogue = null;
        _dialogues[CurrentDialogue].LineIndex = 0;
        DialogueManager.Instance.ShowDialogBoc(false);
    }
}

[Serializable]
public struct Dialog{
    [SerializeField]Line[] lines;
    public int LineIndex;
    public bool AutomaticNextLine;

    public string CurrentLine {get {
        return LineIndex < lines.Length ? lines[LineIndex].text : null;}}

    public float CurrentLineTime {
        get{
            return LineIndex < lines.Length  ? lines[LineIndex]._lineTime : -1;}
    }
    public Sprite CurrentIcon {get {
        if(LineIndex < lines.Length)
        return lines[LineIndex].IsKnown ? lines[LineIndex].character.Icon : lines[LineIndex].character.SilhouetedIcon;
        else return null;
    }}

    
}

[Serializable]
public struct Line{
    [TextArea]
    public string text;
    public float _lineTime;
    public bool IsKnown;
    public Character character;
    
}

