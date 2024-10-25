using JetBrains.Annotations;
using UnityEngine;

[System.Serializable]
public class CharacterConfig
{
    public string id;
    public string display_name;
    public Color bubble_color;
    public Material bubble_material;
}

[CreateAssetMenu(fileName = "CharacterDialogueConfig", menuName = "Scriptable Objects/CharacterDialogueConfig")]
public class CharacterListConfig : ScriptableObject
{
    public CharacterConfig[] configs;
}
