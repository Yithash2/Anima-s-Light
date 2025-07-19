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

    private DialogRuntime _runtime;
    private bool _spelling;
    
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }
    
    public void StartDialog(Dialog data)
    {
        _runtime = new DialogRuntime(data);
        _runtime.Start();
        SubscribeToCharacterEvents();

        _dialogueElements.SetActive(true);
        DisplayLine();
    }

    private void SubscribeToCharacterEvents()
    {
        foreach (var pair in _runtime.CharacterMap)
            pair.Value.SpriteChanged += UpdateImages;
    }

    private void UnsubscribeCharacterEvents()
    {
        foreach (var pair in _runtime.CharacterMap)
            pair.Value.SpriteChanged -= UpdateImages;
    }

    private void Update()
    {
        if (_runtime is null) return;
        
        _runtime?.Update(Time.deltaTime);

        if (InputsManager.Instance.Jump.triggered && !_spelling)
        {
            _runtime.CurrentLine.Events?.Invoke();
            _eventDispatcher?.Dispatch(_runtime.CurrentLine.SceneEvent);

            _runtime.NextLine();
            if (_runtime.IsFinished)
                EndDialog();
            else
                DisplayLine();
        }
    }

    private void DisplayLine()
    {
        var line = _runtime.CurrentLine;
        var speaker = _runtime.CharacterMap[line.Speaker];

        GUINameText.text = line.Speaker.CharacterName;
        GUIText.text = "";
        StartCoroutine(TypeText(line.Text, line.Speed));
        UpdateImages();
    }

    private IEnumerator TypeText(string text, float speed)
    {
        _spelling = true;
        float delay = speed / text.Length;

        foreach (char c in text)
        {
            if (InputsManager.Instance.Jump.triggered)
            {
                GUIText.text = text;
                break;
            }
            GUIText.text += c;
            yield return new WaitForSeconds(delay);
        }

        _spelling = false;
    }

    private void UpdateImages()
    {
        var line = _runtime.CurrentLine;
        GUISpeakerImage.sprite = _runtime.CharacterMap[line.Speaker].GetSprite();
        GUIListenerImage.sprite = _runtime.CharacterMap[line.Listener].GetSprite();
    }

    private void EndDialog()
    {
        UnsubscribeCharacterEvents();
        _dialogueElements.SetActive(false);
        _runtime = null;
        InteractableManager.Instance.StopInteraction();
    }
}
