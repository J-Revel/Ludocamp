using System.Collections;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(ScreenRoot))]
public class ScreenMovementTransition : MonoBehaviour, IScreenAppearTransition, IScreenDisappearTransition
{
    public float2 appear_offset;
    public float appear_rotation;
    public float appear_duration = 0.5f;
    public IEnumerator AppearTransitionCoroutine()
    {
        RectTransform rect_transform = GetComponent<RectTransform>();
        for(float time=0; time < appear_duration; time += Time.unscaledDeltaTime)
        {
            float f = time / appear_duration;
            f = f * f;
            f = 1 - (1-f)*(1-f);
            float2 position = math.lerp(appear_offset, new float2(0, 0), f);
            float angle = math.lerp(appear_rotation, 0, f);
            rect_transform.anchoredPosition = position;
            rect_transform.localRotation = quaternion.Euler(0, 0, angle * math.PI / 180);
            yield return null;
        }
        rect_transform.anchoredPosition = float2.zero;
        rect_transform.localRotation = quaternion.identity;
        yield return null;
    }

    public IEnumerator DisappearTransitionCoroutine()
    {
        RectTransform rect_transform = GetComponent<RectTransform>();
        for(float time=0; time < appear_duration; time += Time.unscaledDeltaTime)
        {
            float f = time / appear_duration;
            f = f * f;
            float2 position = math.lerp(appear_offset, new float2(0, 0), 1-f);
            float angle = math.lerp(appear_rotation, 0, 1-f);
            rect_transform.anchoredPosition = position;
            rect_transform.localRotation = quaternion.Euler(0, 0, angle * math.PI / 180);
            yield return null;
        }
        yield return null;
        Destroy(gameObject);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
