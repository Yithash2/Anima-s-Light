using System;
using UnityEngine;
using UnityEngine.Events;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "Dialog", menuName = "DIALOG/Dialog")]
    public class Dialog : ScriptableObject
    {
        [field: SerializeField] public Line[] Lines { get; private set; }

        public Line CurrentLine => (LineIndex >= 0 && LineIndex < Lines?.Length) ? Lines[LineIndex] : default;
        public Line PreviousLine => (LineIndex > 0 && LineIndex - 1 < Lines?.Length) ? Lines[LineIndex - 1] : default;

        public int LineIndex {get;private set;}

        public Action EndDialog;
        public Action ChangeSpriteSpeaker;
        public Action ChangeSpriteListener;

        public void Start()
        {
            LineIndex = 0;
            CurrentLine.Listener.ChangeExpression((int)CurrentLine.ListenerExpression);
            CurrentLine.Speaker.ChangeExpression((int)CurrentLine.SpeakerExpression);
        }

        public void NextLine()
        {
            LineIndex++;

            if (LineIndex >= Lines?.Length)
            {
                EndDialog?.Invoke();
                LineIndex = 0;
                return;
            }

            ApplyLineVisuals();
        }

        private void ApplyLineVisuals()
        {
            if (Lines == null || Lines.Length == 0 || LineIndex >= Lines.Length) return;

            var line = CurrentLine;
            var prev = (LineIndex > 0) ? PreviousLine : line;

            if (line.ListenerExpression != prev.ListenerExpression)
                line.Listener.ChangeExpression((int)line.ListenerExpression);

            if (line.SpeakerExpression != prev.SpeakerExpression)
                line.Speaker.ChangeExpression((int)line.SpeakerExpression);

            if (!line.Speaker.Equals(prev.Speaker))
                ChangeSpriteSpeaker?.Invoke();

            if (!line.Listener.Equals(prev.Listener))
                ChangeSpriteListener?.Invoke();
        }

        public void UpdateAnimation(float deltaTime)
        {
            if (Lines == null || Lines.Length == 0 || LineIndex >= Lines.Length) return;

            CurrentLine.Speaker?.UpdateAnimation(deltaTime);
            CurrentLine.Listener?.UpdateAnimation(deltaTime);
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
