using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI _dialogText;

    [SerializeField]
    GameObject _dialogBox;

    private Queue _queue = new Queue();
    public static DialogueManager Instance {get; protected set;}
    void Awake()
    {
        if(Instance != null){
            Debug.Log("Found more then one TotalDeathCounter, and destoyed it");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    void Start(){
        _dialogBox.SetActive(false);
    }

    public void UpdateDialog(string text, float lineSpeed){
        char[] letters = text.ToCharArray();
        foreach(char c in letters){
            _queue.Enqueue(c);
        }

        StartCoroutine(ShowText(lineSpeed, _queue.Count));
        
    }

    IEnumerator ShowText(float lineSpeed, int charNum){
        _dialogText.text = "";
        while(_queue.Count != 0){
            _dialogText.text += _queue.Dequeue();
            yield return new WaitForSeconds(lineSpeed/charNum);
        }
    }

    public void ShowDialogBoc(bool condition){
        _dialogBox.SetActive(condition);
    }

    
}
