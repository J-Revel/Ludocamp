using UnityEngine;

[CreateAssetMenu(fileName = "DialogueConfig", menuName = "Scriptable Objects/DialogueConfig")]
public class DialogueConfig : UnlockableConfig
{
    public enum DialogueLineSlot
    {
        FirstPerson, Interlocutor,
    }

    [System.Serializable]
    public struct DialogueLine
    {
        public DialogueLineSlot slot;
        public string text;
    }

    public string character_name;
    public Color character_color;
    
    public DialogueLine[] lines;
    public string[] unlocks;
}
