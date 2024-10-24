using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SlideViewer : MonoBehaviour
{
    [SerializeField] private float transitionDuration;
    [SerializeField] private bool closeScreenOnFinish;
    [SerializeField] private RectTransform slideContainer;
    public event System.Action LastSlideFinished;
    private int slideIndex;
    private int totalSlideCount;
    private bool isTransitioning;
    private CanvasGroup[] slides;
    private CanvasGroup previousSlide;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        totalSlideCount = slideContainer.childCount;
        slides = new CanvasGroup[totalSlideCount];
        for (int i = 0; i < totalSlideCount; i++)
        {
            Transform child = slideContainer.GetChild(i);
            slides[i] = child.GetComponent<CanvasGroup>() ?? child.gameObject.AddComponent<CanvasGroup>();
            slides[i].alpha = 0f;
        }

        StartCoroutine(FadeToSlide(0));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            TryTransitionToNextSlide();
        }
    }

    private void TryTransitionToNextSlide()
    {
        if (isTransitioning) return;

        if (slideIndex >= totalSlideCount - 1)
        {
            LastSlideFinished?.Invoke();
            if(closeScreenOnFinish)
            {
                ScreenTransitionManager.instance.CloseScreen();
            }
            return;
        }

        StartCoroutine(FadeToSlide(slideIndex + 1));
    }

    private IEnumerator FadeToSlide(int index)
    {
        slideIndex = index;
        CanvasGroup slide = slides[slideIndex];
        isTransitioning = true;
        for (float t = 0; t< transitionDuration;t+=Time.unscaledDeltaTime)
        {
            float f = t / transitionDuration;
            slide.alpha = f;
            if(previousSlide)
            {
                previousSlide.alpha = 1 - f;
            }
            yield return null;
        }
        slide.alpha = 1;
        if (previousSlide)
        {
            previousSlide.alpha = 0;
        }
        isTransitioning = false;
        previousSlide = slide;
    }
    
}
