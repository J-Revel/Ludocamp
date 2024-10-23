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
    public char location_id = 'A';
    public int dialogue_index;
    public DialogueEntryData[] lines;
    private int cursor = 0;
    public DialogueBubble bubble_prefab;
    public RectTransform text_popup_container;
    private Unity.Mathematics.Random random;
    private DialogueBubble active_bubble;


    void Start()
    {
        random = new Unity.Mathematics.Random((uint)DateTime.Now.Ticks);
        lines = DialogueDatabase.instance.GetLines(location_id, dialogue_index);
        ShowNext();
    }

    void Update()
    {
    }

    public void ShowNext()
    {
        DialogueEntryData current_line = lines[cursor];
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
