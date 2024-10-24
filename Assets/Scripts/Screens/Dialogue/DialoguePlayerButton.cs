using UnityEngine;

public class DialoguePlayerButton : MonoBehaviour
{
    public DialogueData dialogue;
    public TMPro.TextMeshProUGUI text;
    public ScreenRoot screen_prefab;

    public void Start()
    {
        text.text = dialogue.topic;
    }

    public void OpenDialogue()
    {
        ScreenRoot screen_root = ScreenTransitionManager.instance.InstantiateScreen(screen_prefab, ScreenStackMode.Push);
        DialogueScreen dialogue_screen = screen_root.GetComponent<DialogueScreen>();
        dialogue_screen.dialogue = dialogue;
    }
}
