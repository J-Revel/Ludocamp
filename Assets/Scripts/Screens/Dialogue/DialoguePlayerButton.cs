using UnityEngine;

public class DialoguePlayerButton : MonoBehaviour
{
    public DialogueData dialogue;
    public TMPro.TextMeshProUGUI text;
    public ScreenRoot screen_prefab;
    public GameObject not_viewed_display;

    public void Start()
    {
        text.text = dialogue.topic;
        not_viewed_display.SetActive(!EvaluationReport.Instance.IsDialogueViewed(dialogue.uid));
    }

    public void OpenDialogue()
    {
        EvaluationReport.Instance.RemoveNotViewedDialogue(dialogue.uid);
        not_viewed_display.SetActive(false);
        ScreenRoot screen_root = ScreenTransitionManager.instance.InstantiateScreen(screen_prefab, ScreenStackMode.Push);
        DialogueScreen dialogue_screen = screen_root.GetComponent<DialogueScreen>();
        dialogue_screen.dialogue = dialogue;
    }
}
