using UnityEngine;
using System.Collections;
using System;

namespace Dialogue
{
	[CreateAssetMenu(fileName = "Character", menuName = "DIALOG/Character")]
	public class Character : ScriptableObject
	{
		public Sprite CharacterSprite { get {return _expressionSprite[(int)_expression];}}
        [field: SerializeField] public string CharacterName { get; private set; }

        [SerializeField] private Sprite[] _expressionSprite = new Sprite[5];

        private Expression _expression;

        public void ChangeExpression(int i)
        {
            if (i >= 5 || i < 0) return;

            _expression = (Expression)i;
        }

        // override object.Equals
        public bool Equals(Character obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return obj.CharacterName == this.CharacterName;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return HashCode.Combine(CharacterName, CharacterSprite);
        }
	}

    public enum Expression
    {
        Neutral = 0,
        Enjoyed = 1,
        Angry = 2,
        Smug = 3,
        Afraid = 4
    }
}