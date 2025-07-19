using Dialogue;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance { get; private set; }

    [SerializeField] private GameObject _dialogueElements;
    [SerializeField] private TextMeshProUGUI GUINameText;
    [SerializeField] private Image GUISpeakerImage;
    [SerializeField] private Image GUIListenerImage;
    [SerializeField] private TextMeshProUGUI GUIText;
    
    [SerializeField] private LineEventDispatcher _eventDispatcher;

    private Dialog _currentDialog;
    private bool _isSpelling;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    public void StartDialog(Dialog dialog)
    {
        if (dialog == null || dialog.Lines == null || dialog.Lines.Length == 0)
        {
            Debug.LogError("Dialog is null or has no lines.");
            return;
        }

        _dialogueElements.SetActive(true);
        _currentDialog = dialog;

        _currentDialog.ChangeSpriteListener += SetListener;
        _currentDialog.ChangeSpriteSpeaker += SetSpeaker;
        _currentDialog.EndDialog += EndDialog;

        _currentDialog.Start();

        SetListener();
        SetSpeaker();


        NextLine();
    }
    
    private void SubscribeToCharacterImageChanges()
    {
        _currentDialog.CurrentLine.Speaker.ImageChanged += SetSpeaker;
        _currentDialog.CurrentLine.Listener.ImageChanged += SetListener;
    }
    
    private void UnSubscribeToCharacterImageChanges()
    {
        _currentDialog.PreviousLine.Speaker.ImageChanged -= SetSpeaker;
        _currentDialog.PreviousLine.Listener.ImageChanged -= SetListener;
    }

    public void CheckIfInterrupted()
    {
        if (_isSpelling) _isSpelling = false;
        else
        {
            _currentDialog.NextLine();
            NextLine();
        }
    }

    private void NextLine()
    {
        if (_currentDialog == null) return;
        
        
        if(_currentDialog.LineIndex != 0) UnSubscribeToCharacterImageChanges();
        SubscribeToCharacterImageChanges();
        
        _isSpelling = true;
        GUIText.text = "";
        // Invoke UnityEvent (non-scene)
        _currentDialog.CurrentLine.Events?.Invoke();

        // Dispatch scene-based event
        _eventDispatcher?.Dispatch(_currentDialog.CurrentLine.SceneEvent);
        StartCoroutine(SpellText());
    }

    private IEnumerator SpellText()
    {
        var line = _currentDialog.CurrentLine;
        float timePerChar = line.Speed / line.Text.Length;

        foreach (char c in line.Text)
        {
            GUIText.text += c;
            if (!_isSpelling)
            {
                GUIText.text = line.Text;
                break;
            }
            yield return new WaitForSeconds(timePerChar);
        }
        

        _isSpelling = false;
    }


    private void SetSpeaker()
    {
        GUINameText.text = _currentDialog.CurrentLine.Speaker.CharacterName;
        GUISpeakerImage.sprite = _currentDialog.CurrentLine.Speaker.CharacterSprite;
    }

    private void SetListener()
    {
        GUIListenerImage.sprite = _currentDialog.CurrentLine.Listener.CharacterSprite;
    }

    private void EndDialog()
    {
        _dialogueElements.SetActive(false);

        _currentDialog.ChangeSpriteListener -= SetListener;
        _currentDialog.ChangeSpriteSpeaker -= SetSpeaker;
        _currentDialog.EndDialog -= EndDialog;
        _currentDialog = null;

        GUIText.text = "";
        InteractableManager.Instance.StopInteraction();
    }

    private void Update()
    {
        if (_currentDialog == null) return;

        _currentDialog.UpdateAnimation(Time.deltaTime);

        if (InputsManager.Instance.Jump.triggered)
            CheckIfInterrupted();
    }
}
