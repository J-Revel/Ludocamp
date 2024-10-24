using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
//using UnityEditor.Localization;
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

[System.Serializable]
public struct LocationCharacter
{
    public char location_id;
    public GameObject character_display;
}

public class DialogueScreen : MonoBehaviour
{
    public DialogueData dialogue;
    public DialogueEntryData[] lines;
    private int cursor = 0;
    public DialogueBubble bubble_prefab;
    public RectTransform text_popup_container;
    private Unity.Mathematics.Random random;
    private DialogueBubble active_bubble;
    public LocationCharacter[] characters;
    public float2 min_offset = new float2(0.2f, 0.5f);
    private float2 previous_offset;

    void Start()
    {
        random = new Unity.Mathematics.Random((uint)DateTime.Now.Ticks);
        lines = DialogueDatabase.instance.GetLines(dialogue.location_id, dialogue.dialogue_index);
        foreach(var character in characters)
        {
            character.character_display.SetActive(character.location_id == dialogue.location_id);
        }
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

        float2 new_position = random.NextFloat2(new float2(1, 1));
        while(math.abs(new_position.x - previous_offset.x) < min_offset.x || math.abs(new_position.y - previous_offset.y) < min_offset.y)
        {
            new_position = random.NextFloat2(new float2(1, 1));
        }
        bubble.position = new_position;
        bubble.dialogue_line = current_line;
        previous_offset = new_position;
        active_bubble = bubble;

        cursor++;
        if(lines.Length <= cursor)
        {
            ScreenTransitionManager.instance.CloseScreen();
        }
    }
}
