using UnityEngine;

public class DialoguePlayerButton : MonoBehaviour
{
    public char location_id = 'A';
    public int dialogue_index = 1;
    public ScreenRoot screen_prefab;

    public void OpenDialogue()
    {
        ScreenRoot screen_root = ScreenTransitionManager.instance.InstantiateScreen(screen_prefab, ScreenStackMode.Push);
        DialogueScreen dialogue_screen = screen_root.GetComponent<DialogueScreen>();
        dialogue_screen.dialogue_index = dialogue_index;
        dialogue_screen.location_id = location_id;
    }
}
