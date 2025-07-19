using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "Dialog", menuName = "DIALOG/Dialog")]
    public class Dialog : ScriptableObject
    {
        [field: SerializeField] public Line[] Lines { get; private set; }
    }
    
    public class DialogRuntime
    {
        private Dialog _data;
        private int _lineIndex;

        public Line CurrentLine => _data.Lines[_lineIndex];
        public bool IsFinished => _lineIndex >= _data.Lines.Length;

        public Dictionary<Character, CharacterRuntime> CharacterMap { get; private set; }

        public DialogRuntime(Dialog data)
        {
            _data = data;
            _lineIndex = 0;
            CharacterMap = new();

            foreach (var line in _data.Lines)
            {
                if (!CharacterMap.ContainsKey(line.Speaker))
                    CharacterMap[line.Speaker] = new CharacterRuntime(line.Speaker);
                if (!CharacterMap.ContainsKey(line.Listener))
                    CharacterMap[line.Listener] = new CharacterRuntime(line.Listener);
            }
        }

        public void Start()
        {
            ApplyExpressions(CurrentLine);
        }

        public void NextLine()
        {
            _lineIndex++;
            if (!IsFinished)
                ApplyExpressions(CurrentLine);
        }

        private void ApplyExpressions(Line line)
        {
            CharacterMap[line.Speaker].SetExpression(line.SpeakerExpression);
            CharacterMap[line.Listener].SetExpression(line.ListenerExpression);
        }

        public void Update(float deltaTime)
        {
            foreach (var character in CharacterMap.Values)
                character.Update(deltaTime);
        }
    }

    
    public enum LineEventType
    {
        None,
        StopMice,
        ShareCheese,
        BecomeCheese,
        TeleportPlayerEasterEgg,
    }

    [Serializable]
    public struct Line
    {
        [field: SerializeField, TextArea] public string Text { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public Character Speaker { get; private set; }
        [field: SerializeField] public Expression SpeakerExpression { get; private set; }
        [field: SerializeField] public Character Listener { get; private set; }
        [field: SerializeField] public Expression ListenerExpression { get; private set; }

        [field: SerializeField] public UnityEvent Events { get; private set; }
        [field: SerializeField] public LineEventType SceneEvent { get; private set; }
    }
} 
