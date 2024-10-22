using UnityEngine;

[CreateAssetMenu(fileName = "MapPointConfig", menuName = "Scriptable Objects/MapPointConfig")]
public class MapPointConfig : ScriptableObject
{
    public string name;
    public DialogueConfig[] dialogues;
}
