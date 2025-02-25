using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI _dialogText;

    [SerializeField]
    GameObject _dialogBox;

    [SerializeField]
    Image _characterIcon;

    private Queue _queue = new Queue();

    public bool CanTalk = true;
    

    [HideInInspector]
    public Dialogue CurrentDialogue;
    public static DialogueManager Instance {get; protected set;}
    void Awake()
    {
        if(Instance != null){
            Debug.Log("Found more then one DialogueManager, and destoyed it");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    void Start(){
        _dialogBox.SetActive(false);
    }

    public void UpdateDialog(string text, float lineSpeed, Sprite caraIcon){
        _characterIcon.sprite = caraIcon;
        char[] letters = text.ToCharArray();
        foreach(char c in letters){
            _queue.Enqueue(c);
        }

        _queue.Enqueue("0");

        StartCoroutine(ShowText(lineSpeed, _queue.Count));
        
    }

    IEnumerator ShowText(float lineSpeed, int charNum){
        _dialogText.text = "";
        while(_queue.Count != 1){
            _dialogText.text += _queue.Dequeue();
            yield return new WaitForSeconds(lineSpeed/charNum);
        }
        CurrentDialogue.CurrentLine.Events?.Invoke();
        CurrentDialogue.CurrentLineIndex++;

        _queue.Dequeue();

    }

    public void ShowDialogBoc(bool condition){
        _dialogBox.SetActive(condition);
    }

    public void Next(InputAction.CallbackContext ctx){
        if (!CanTalk) return;

        if (ctx.started){
            if(CurrentDialogue!=null && _queue.Count == 0) NextLine();
        }
    }

    public void NextLine()
    {
        CurrentDialogue.NextLine();
    }

    public void ChangeContinueDialogue(bool value)
    {
        CanTalk = value;
    }


}
