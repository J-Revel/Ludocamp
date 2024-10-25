using System.Collections;
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

    void Start()
    {
        title_display.text = config.title;
        button.onClick.AddListener(() => {
            open = !open;
        });
        DialogueData[] dialogues = DialogueDatabase.instance.ListAvailableDialogues(config.location_id);
        for(int i=0; i<dialogues.Length; i++)
        {
            DialoguePlayerButton button = Instantiate(button_prefab, container);
            button.dialogue = dialogues[i];
        }
    }
}
