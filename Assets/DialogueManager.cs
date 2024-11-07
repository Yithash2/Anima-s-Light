using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI _dialogText;
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

    public void UpdateDialog(string text){
        _dialogText.text = text;
    }

    
}
