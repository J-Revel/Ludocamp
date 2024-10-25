using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueList : MonoBehaviour
{
    public Button button;
    public MapPointConfig config;
    public DialoguePlayerButton button_prefab;
    public RectTransform container;
    public bool open;
    public float animation_duration = 0.5f;
    public TMPro.TextMeshProUGUI title_display;
    private List<DialoguePlayerButton> visible_buttons = new List<DialoguePlayerButton>();

    void Start()
    {
        GetComponentInParent<ScreenRoot>().return_to_screen_delegate += () =>
        {
            UpdateDisplay();
        };
        UpdateDisplay();
        title_display.text = config.title;
        button.onClick.AddListener(() => {
            open = !open;
        });
    }

    void UpdateDisplay()
    {
        foreach(var button in visible_buttons)
        {
            Destroy(button.gameObject);
        }
        visible_buttons.Clear();
        DialogueData[] dialogues = DialogueDatabase.instance.ListAvailableDialogues(config.location_id);
        for(int i=0; i<dialogues.Length; i++)
        {
            DialoguePlayerButton button = Instantiate(button_prefab, container);
            button.dialogue = dialogues[i];
            visible_buttons.Add(button);
        }
        if (dialogues.Length == 0)
            gameObject.SetActive(false);

    }
}
