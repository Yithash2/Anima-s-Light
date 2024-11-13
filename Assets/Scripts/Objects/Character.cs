using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Characters", menuName = "ScriptableObject/Characters")]
public class Character : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public Sprite SilhouetedIcon;
}
