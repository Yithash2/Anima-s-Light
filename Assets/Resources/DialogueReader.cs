using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Dialogue : MonoBehaviour
{

    [SerializeField]
    Line[] _dialogues;
    public int CurrentLineIndex;
    public Line CurrentLine { get { return _dialogues[CurrentLineIndex]; } }

    [ContextMenu("NextLine")]
    public void NextLine(){
        if(DialogueManager.Instance.CurrentDialogue == null || DialogueManager.Instance.CurrentDialogue == this){
            DialogueManager.Instance.CurrentDialogue = this;
            if(CurrentLineIndex >= _dialogues.Length)
            {
                EndDialog();
                return;
            }else{
                DialogueManager.Instance.UpdateDialog(_dialogues[CurrentLineIndex].text, _dialogues[CurrentLineIndex].LineTime, _dialogues[CurrentLineIndex].Icon);
                DialogueManager.Instance.ShowDialogBoc(true);
            }
        }
    }

    public void EndDialog(){
        DialogueManager.Instance.CurrentDialogue = null;
        CurrentLineIndex = 0;
        DialogueManager.Instance.ShowDialogBoc(false);
    }
}

[Serializable]
public struct Line{
    [TextArea]
    public string text;
    public float LineTime;
    public bool IsKnown;

    [SerializeField] private Character character;

    public Sprite Icon { get { return IsKnown ? character.Icon : character.SilhouetedIcon; } }

    public UnityEvent Events;
    
}

