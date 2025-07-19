using System;
using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "Character", menuName = "DIALOG/Character")]
    public class Character : ScriptableObject
    {
        [SerializeField] private AnimatedImages[] expressionSprites = new AnimatedImages[5];
        [field: SerializeField] public string CharacterName { get; private set; }

        private Expression _expression;
        public event Action ImageChanged;

        public Sprite CharacterSprite => expressionSprites[(int)_expression].CurrentSprite();

        public void ChangeExpression(int index)
        {
            if (index < 0 || index >= expressionSprites.Length) return;
            _expression = (Expression)index;
            expressionSprites[index].Reset();
            expressionSprites[index].ImageChanged += () => ImageChanged?.Invoke();
            ImageChanged?.Invoke();
        }

        public void UpdateAnimation(float deltaTime)
        {
            expressionSprites[(int)_expression].UpdateAnimation(deltaTime);
        }

        public override bool Equals(object obj)
        {
            return obj is Character c && c.CharacterName == CharacterName;
        }

        public override int GetHashCode() => CharacterName.GetHashCode();
    }

    public enum Expression { Neutral, Enjoyed, Angry, Smug, Afraid }

    [Serializable]
    public class AnimatedImages
    {
        public Sprite[] Images;
        public int FramesPerSecond = 4;
        [SerializeField] private bool loop = true;

        private float _elapsedTime;
        private int _currentIndex;

        public event Action ImageChanged;

        public void UpdateAnimation(float deltaTime)
        {
            if (Images == null || Images.Length == 0 || FramesPerSecond <= 0) return;

            // Single image case: nothing to animate
            if (Images.Length == 1)
            {
                _currentIndex = 0;
                return;
            }

            if (!loop && _currentIndex >= Images.Length - 1) return;

            _elapsedTime += deltaTime;
            float frameDuration = 1f / FramesPerSecond;

            if (_elapsedTime >= frameDuration)
            {
                _elapsedTime -= frameDuration;
                _currentIndex = (_currentIndex + 1) % Images.Length;
                ImageChanged?.Invoke();
            }
        }
        
        

        public Sprite CurrentSprite() => (Images != null && Images.Length > 0) ? Images[Mathf.Clamp(_currentIndex, 0, Images.Length - 1)] : null;

        public void Reset()
        {
            _elapsedTime = 0f;
            _currentIndex = 0;
        }
    }
}