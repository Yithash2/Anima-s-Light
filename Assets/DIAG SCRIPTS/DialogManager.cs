using Dialogue;
using System.Collections;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    public void SetSpeaker(Character chara)
    {
        GUINameText.text = chara.CharacterName;
        GUISpeakerImage.sprite = chara.CharacterSprite;
    }

    public void SetExpression(Character chara, int i)
    {
        chara.ChangeExpression(i);
    }

    public void SetListener(Character chara)
    {
        GUIListenerImage.sprite = chara.CharacterSprite;
    }

    public void StartDialog(Dialog diag)
    {
        _dialogueElements.SetActive(true);
        _currentDialog = diag;
        _lineIndex = 0;
        NextLine();
    }

    [ContextMenu("NL")]
    public void CheckIfInterupted()
    {
        if (_isSpelling)
        {
            _isSpelling = false;
        }
        else
        {
            NextLine();
        }
    }

    private void NextLine()
    {
        if (!_currentDialog) return;

        if (_lineIndex >= _currentDialog.Lines.Length)
        {
            EndDialog();
            return;
        }

        _isSpelling = true;


        GUIText.text = "";

        if (_lineIndex == 0)
        {
            SetExpression(_currentLine.Speaker, (int)_currentLine.SpeakerExpression);
            SetSpeaker(_currentLine.Speaker);
            SetExpression(_currentLine.Listener, (int)_currentLine.ListenerExpression);
            SetListener(_currentLine.Listener);
        }
        else
        {
            if (_currentLine.ListenerExpression != _prevLine.ListenerExpression)
                SetExpression(_currentLine.Listener, (int)_currentLine.ListenerExpression);

            if (_currentLine.SpeakerExpression != _prevLine.SpeakerExpression)
                SetExpression(_currentLine.Speaker, (int)_currentLine.SpeakerExpression);

            if (!_currentLine.Speaker.Equals(_prevLine.Speaker))
                SetSpeaker(_currentLine.Speaker);

            if (!_currentLine.Listener.Equals(_prevLine.Listener))
                SetListener(_currentLine.Listener);
            
            
        }
        
        StartCoroutine(SpellText());
    }

    private IEnumerator SpellText()
    {

        float t4char = _currentLine.speed / _currentLine.text.Length;

        foreach(char c in _currentLine.text)
        {
            GUIText.text += c;

            if (!_isSpelling)
            {
                GUIText.text = _currentLine.text;
                break;
            }

            yield return new WaitForSeconds(t4char);
        }

        _lineIndex++;
        _isSpelling = false;

    }

    private void EndDialog()
    {
        _dialogueElements.SetActive(false);
        _currentDialog = null;
        InteractableManager.Instance.StopInteraction();
        _lineIndex = 0;
        GUIText.text = "";
    }

    private void Update()
    {
        if (InputsManager.Instance.Jump.triggered)
        {
            CheckIfInterupted();
        }
    }

    [SerializeField] GameObject _dialogueElements;
    [SerializeField] TextMeshProUGUI GUINameText;
    [SerializeField] Image GUISpeakerImage;
    [SerializeField] Image GUIListenerImage;
    [SerializeField] TextMeshProUGUI GUIText;

    private bool _isSpelling = false;
    private Dialog _currentDialog = null;
    private Line _currentLine { get { return _currentDialog.Lines[_lineIndex]; } }
    private Line _prevLine { get { return _currentDialog.Lines[_lineIndex - 1]; } }

    private int _lineIndex = 0;

    [SerializeField] Dialog temp_diag;
}
