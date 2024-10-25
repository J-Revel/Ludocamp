using UnityEngine;

public class TutoDialoguePlayer : MonoBehaviour
{
    public int block_unlock_index = 1;
    public ScreenRoot screen_prefab;
    public DialogueData dialogue;

    void Start()
    {
        EvaluationReport.Instance.BlockValidated += (int block_index) =>
        {
            if (block_index == block_unlock_index)
            {
                ScreenRoot screen = ScreenTransitionManager.instance.InstantiateScreen(screen_prefab, ScreenStackMode.Push);
                screen.GetComponent<DialogueScreen>().dialogue = dialogue;
            }
        };
    }
}
