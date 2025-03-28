using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil.Cil;
using Unity.Mathematics;
//using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

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
    public DocumentConfig[] documents;
    public DialogueData dialogue;
    public MapPointConfig[] map_points;
    public DialogueEntryData[] lines;
    private int cursor = 0;
    public DialogueBubble bubble_prefab;
    public RectTransform text_popup_container;
    private Unity.Mathematics.Random random;
    private DialogueBubble active_bubble;
    public LocationCharacter[] characters;
    public float2 min_offset = new float2(0.2f, 0.5f);
    private float2 previous_offset;
    
    public ScreenRoot docViewerScreenPrefab;

    public Button new_document_button_prefab;
    public RectTransform new_document_button_panel;
    
    private List<string> new_documents = new List<string>();
    private List<Button> new_document_buttons = new List<Button>();

    void Start()
    {
        GetComponent<ScreenRoot>().screen_close_delegate += () =>
        {
            AmbianceManager.Instance.ResetToBaseAmbiance();
        };
        foreach(var point in map_points)
        {
            if (point.location_id == dialogue.location_id)
                AmbianceManager.Instance.ChangeAmbiance(point.ambiance);
        }
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
        if(lines.Length <= cursor)
        {
            ScreenTransitionManager.instance.CloseScreen();
            return;
        }
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
        if (current_line.unlocks == null)
            return;
            
        foreach(string unlock in current_line.unlocks)
        {
            if (unlock.ToLower().StartsWith("doc"))
            {
                new_documents.Add(unlock);
                Button new_doc_button = Instantiate(new_document_button_prefab, new_document_button_panel);
                foreach (DocumentConfig doc in documents)
                {
                    if(doc.id == unlock)
                        new_doc_button.GetComponentInChildren<TMPro.TMP_Text>().text = doc.fullTitle;
                }
                new_document_buttons.Add(new_doc_button);
                new_documents.Add(unlock);
                new_doc_button.onClick.AddListener(() =>
                {
                    ScreenRoot docViewer = ScreenTransitionManager.instance.InstantiateScreen(docViewerScreenPrefab, ScreenStackMode.Push);
                    docViewer.GetComponentInChildren<DocViewer>().OpenDocument(unlock);
                });
                new_document_button_panel.gameObject.SetActive(true);
            }
        }

    }
}
