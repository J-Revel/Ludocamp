using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

[System.Serializable]
public struct DialogueLine
{
    public string character;
    public int index;
    public string text;
}

public class DialogueScreen : MonoBehaviour
{
    public string dialogue_id;
    public TableReference table;
    public DialogueLine[] lines;
    private int cursor = 0;
    public DialogueBubble bubble_prefab;
    public RectTransform text_popup_container;
    private Unity.Mathematics.Random random;
    private DialogueBubble active_bubble;


    void Start()
    {
        random = new Unity.Mathematics.Random((uint)DateTime.Now.Ticks);
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
        ShowNext();
    }

    void Update()
    {
    }

    public void ShowNext()
    {
        DialogueLine current_line = lines[cursor];
        if (active_bubble != null)
            active_bubble.passed = true;

        var bubble = Instantiate(bubble_prefab, text_popup_container);
        bubble.position = random.NextFloat2(new float2(1, 1));
        bubble.dialogue_line = current_line;
        active_bubble = bubble;

        cursor++;
        if(lines.Length <= cursor)
        {
            ScreenTransitionManager.instance.CloseScreen();
        }
    }
}
