using System.Collections;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(ScreenRoot))]
public class ScreenFadeTransition: MonoBehaviour, IScreenAppearTransition, IScreenDisappearTransition
{
    private CanvasGroup canvas_group;
    public float appear_transition_duration;
    public float disappear_transition_duration;

    private void Start()
    {
    }

    public IEnumerator AppearTransitionCoroutine()
    {
        canvas_group = GetComponent<CanvasGroup>();
        if(appear_transition_duration < Mathf.Epsilon)
        {
            canvas_group.alpha = 1;
            yield break;
        }
        for(float time = 0; time < appear_transition_duration; time+=Time.unscaledDeltaTime)
        {
            float f = time / appear_transition_duration;
            canvas_group.alpha = f;
            yield return null;
        }
        canvas_group.alpha = 1;
    }

    public IEnumerator DisappearTransitionCoroutine()
    {
        canvas_group = GetComponent<CanvasGroup>();
        if (disappear_transition_duration < Mathf.Epsilon)
        {
            canvas_group.alpha = 0;
            yield break;
        }
        for (float time = 0; time < disappear_transition_duration; time+=Time.unscaledDeltaTime)
        {
            float f = time / disappear_transition_duration;
            canvas_group.alpha = 1 - f;
            yield return null;
        }
        canvas_group.alpha = 0;
        Destroy(gameObject);
    }
}
