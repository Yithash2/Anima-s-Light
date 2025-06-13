using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "Dialog", menuName = "DIALOG/Dialog")]
    public class Dialog : ScriptableObject
	{
        [field: SerializeField] public Line[] Lines { get; private set; }
	}

    [System.Serializable]
    public struct Line
    {
        [field: SerializeField] public string text { get; private set; }
        [field: SerializeField] public float speed { get; private set; }
        [field: SerializeField] public Character Speaker { get; private set; }
        [field : SerializeField] public Expression SpeakerExpression { get; private set; }
        [field: SerializeField] public Character Listener { get; private set; }
        [field: SerializeField] public Expression ListenerExpression { get; private set; }

        [field: SerializeField] public UnityEvent Events { get; private set; }
    }


}