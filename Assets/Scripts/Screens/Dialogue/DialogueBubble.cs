using JetBrains.Annotations;
using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBubble : MonoBehaviour
{
    public DialogueEntryData dialogue_line;
    public Image background_image;
    public CharacterListConfig characters;
    public TMPro.TextMeshProUGUI text;
    private CanvasGroup canvas_group;
    public bool passed = false;
    public float appear_duration = 1;
    public float disappear_duration = 1;
    public float disappear_delay = 2;
    public float2 position;
    public Color character_color;
    public GameObject dialogue_unlock_display;
    public GameObject document_unlock_display;
    

    IEnumerator Start()
    {
        string character = "";
        for(int i=0; i<characters.configs.Length; i++)
        {
            if (characters.configs[i].id == dialogue_line.character)
            {
                character = characters.configs[i].display_name;
                background_image.color = characters.configs[i].bubble_color;
                background_image.material = characters.configs[i].bubble_material;
            }
        }
        string color_hex = ((int)(character_color.r*255)).ToString("X2");
        color_hex += ((int)(character_color.g*255)).ToString("X2");
        color_hex += ((int)(character_color.b*255)).ToString("X2");
        color_hex += ((int)(character_color.a*255)).ToString("X2");

        string displayed_text = dialogue_line.text.Replace("[", "<color=#dcb200>").Replace("]", "</color>");
        text.text = "<b><color=#" + color_hex + "> " + character + " : </b></color>" + displayed_text;
        canvas_group = GetComponent<CanvasGroup>();
        RectTransform rect_transform = GetComponent<RectTransform>();
        rect_transform.anchorMin = position;
        rect_transform.anchorMax = position;
        rect_transform.pivot = position;
        dialogue_unlock_display.SetActive(false);
        document_unlock_display.SetActive(false);
        if(dialogue_line.unlocks != null)
        {
            foreach (string unlock in dialogue_line.unlocks)
            {
                if(EvaluationReport.Instance.UnlockDialogueOrDocument(unlock))
                {
                    if (unlock.ToLower().StartsWith("doc"))
                        document_unlock_display.SetActive(true);
                    else
                        dialogue_unlock_display.SetActive(true);
                }
            }
        }

        for (float time = 0; time < appear_duration; time += Time.unscaledDeltaTime)
        {
            float f = time / appear_duration;
            canvas_group.alpha = f;
            yield return null;
        }
        canvas_group.alpha = 1;
        float disappear_time = 0;
        while (!passed || disappear_time < disappear_delay)
        {
            disappear_time += Time.deltaTime;
            yield return null;
        }

        for (float time = 0; time < disappear_duration; time += Time.unscaledDeltaTime)
        {
            float f = time / disappear_duration;
            canvas_group.alpha = 1 - f;
            yield return null;
        }
        
        Destroy(gameObject);
    }
}
