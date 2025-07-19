using System;
using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "Character", menuName = "DIALOG/Character")]
    public class Character : ScriptableObject
    {
        [SerializeField] public AnimatedImages[] expressionSprites = new AnimatedImages[5];
        [field: SerializeField] public string CharacterName { get; private set; }
    }
    
    public class CharacterRuntime
    {
        public Character Data { get; private set; }
        private Expression _expression;
        private float _elapsedTime;
        private int _currentIndex;

        public event Action SpriteChanged;

        public CharacterRuntime(Character data)
        {
            Data = data;
        }

        public void SetExpression(Expression expr)
        {
            if ((int)expr >= Data.expressionSprites.Length) return;
            _expression = expr;
            _elapsedTime = 0f;
            _currentIndex = 0;
            SpriteChanged?.Invoke();
        }

        public void Update(float deltaTime)
        {
            var anim = Data.expressionSprites[(int)_expression];
            if (anim.Images == null || anim.Images.Length <= 1 || anim.FramesPerSecond <= 0)
                return;

            _elapsedTime += deltaTime;
            float frameDuration = 1f / anim.FramesPerSecond;

            if (_elapsedTime >= frameDuration)
            {
                _elapsedTime -= frameDuration;

                if (anim.Loop)
                {
                    _currentIndex = (_currentIndex + 1) % anim.Images.Length;
                    SpriteChanged?.Invoke();
                }
                else if (_currentIndex < anim.Images.Length - 1)
                {
                    _currentIndex++;
                    SpriteChanged?.Invoke();
                }
            }
        }

        public Sprite GetSprite()
        {
            var anim = Data.expressionSprites[(int)_expression];
            if (anim.Images == null || anim.Images.Length == 0) return null;
            return anim.Images[Mathf.Clamp(_currentIndex, 0, anim.Images.Length - 1)];
        }
    }




    public enum Expression { Neutral, Enjoyed, Angry, Smug, Afraid }

    [Serializable]
    public class AnimatedImages
    {
        public Sprite[] Images;
        public int FramesPerSecond = 4;
        public bool Loop = true;

        private float _elapsedTime;
        private int _currentIndex;
    }
}