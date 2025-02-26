using UnityEngine;
using System.Collections;

namespace Levels
{
	[CreateAssetMenu(fileName = "Level", menuName = "Level")]
	public class LevelScriptableObj : ScriptableObject
	{
		[field: SerializeField] public string Name { get; private set; }

		[field : SerializeField] public string SceneName { get; private set; }
        [field: SerializeField] public Sprite ActivateSprite { get; private set; }

        [field: SerializeField] public Sprite SpriteDesabledSprite {get; private set;}
		[field: SerializeField] public Sprite WonSprite {get; private set;}
	
	}
}