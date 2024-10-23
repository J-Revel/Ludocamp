using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class DialogueBubble : MonoBehaviour
{
    public DialogueLine dialogue_line;
    public TMPro.TextMeshProUGUI text;
    private CanvasGroup canvas_group;
    public bool passed = false;
    public float appear_duration = 1;
    public float disappear_duration = 1;
    public float disappear_delay = 2;
    

    IEnumerator Start()
    {
        text.text = dialogue_line.text;
        canvas_group = GetComponent<CanvasGroup>();

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
