using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public class DialogueScreen : MonoBehaviour
{
    public string dialogue_id;
    public TableReference table;
    public DialogueLine[] lines;
    private int cursor = 0;

    [System.Serializable]
    public struct DialogueLine
    {
        public string character;
        public int index;
        public string text;
    }

    void Start()
    {
        List<DialogueLine> lines = new List<DialogueLine>();
        foreach (KeyValuePair<long, StringTableEntry> entry in LocalizationSettings.StringDatabase.GetTable(table))
        {
            if(entry.Value.Key.StartsWith(dialogue_id))
            {
                int line_index = int.Parse(entry.Value.Key.Substring(dialogue_id.Length + 1, 3));
                string character_id = entry.Value.Key.Substring(dialogue_id.Length + 5);
                lines.Add(new DialogueLine
                {
                    index = line_index,
                    text = entry.Value.Value,
                    character = character_id,
                });
            }
        }
        lines.Sort((DialogueLine A, DialogueLine B) => { return A.index - B.index; });
        this.lines = new DialogueLine[lines.Count];
        for(int i=0; i<lines.Count; i++)
        {
            this.lines[i] = lines[i];
        }
    }

    void Update()
    {
        
    }

    void ShowNext()
    {

    }
}
