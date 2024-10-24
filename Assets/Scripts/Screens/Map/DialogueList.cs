using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueList : MonoBehaviour
{
    public Button button;
    public char location;
    public string title;
    public DialoguePlayerButton button_prefab;
    public RectTransform container;
    public bool open;
    public Transform animated_element;
    public float animation_duration = 0.5f;
    public TMPro.TextMeshProUGUI title_display;

    IEnumerator Start()
    {
        title_display.text = title;
        button.onClick.AddListener(() => {
            open = !open;
        });
        DialogueData[] dialogues = DialogueDatabase.instance.ListAvailableDialogues(location);
        for(int i=0; i<dialogues.Length; i++)
        {
            DialoguePlayerButton button = Instantiate(button_prefab, container);
            button.dialogue = dialogues[i];
        }
        while(true)
        {
            while(!open) yield return null;
            yield return OpenCoroutine();
            while (open) yield return null;
            yield return CloseCoroutine();
        }
    }
    private IEnumerator OpenCoroutine()
    {
        for(float time = 0; time < animation_duration; time += Time.unscaledDeltaTime)
        {
            float f = time / animation_duration;
            f = f * f;
            animated_element.localScale = new Vector3(f, f, f);
            yield return null;
        }
        animated_element.localScale = Vector3.one;
    }
    private IEnumerator CloseCoroutine()
    {
        for(float time = 0; time < animation_duration; time += Time.unscaledDeltaTime)
        {
            float f = 1 - time / animation_duration;
            f = 1 - (1 - f) * (1 - f);
            animated_element.localScale = new Vector3(f, f, f);
            yield return null;
        }
        animated_element.localScale = Vector3.zero;
    }
}
