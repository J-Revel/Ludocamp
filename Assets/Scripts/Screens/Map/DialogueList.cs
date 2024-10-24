using UnityEngine;

public class DialogueList : MonoBehaviour
{
    public char location;
    public DialoguePlayerButton button_prefab;
    public RectTransform container;

    void Start()
    {
        DialogueData[] dialogues = DialogueDatabase.instance.ListAvailableDialogues(location);
        for(int i=0; i<dialogues.Length; i++)
        {
            DialoguePlayerButton button = Instantiate(button_prefab, container);
            button.dialogue = dialogues[i];
        }
    }
}
