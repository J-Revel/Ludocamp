using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SlideViewer : MonoBehaviour
{
    [SerializeField] private float transitionDuration;
    [SerializeField] private OnFinishAction onFinishAction;
    [SerializeField] private RectTransform slideContainer;
    public ScreenRoot next_screen;
    public enum OnFinishAction
    {
        CloseScreen,
        OpenScreen,
        ExitGame
    }
    public event System.Action LastSlideFinished;
    private int slideIndex;
    private int totalSlideCount;
    private bool isTransitioning;
    private CanvasGroup[] slides;
    private CanvasGroup previousSlide;
    private float transitionTimer;

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
        if (isTransitioning)
        {
            transitionTimer = transitionDuration;
            return;
        }

        if (slideIndex >= totalSlideCount - 1)
        {
            isTransitioning = true;

            LastSlideFinished?.Invoke();

            switch(onFinishAction)
            {
                case OnFinishAction.CloseScreen:
                    ScreenTransitionManager.instance.CloseScreen();
                    break;
                case OnFinishAction.OpenScreen:
                    ScreenTransitionManager.instance.ShowScreen(next_screen, ScreenStackMode.Replace);
                    break;

                case OnFinishAction.ExitGame:
                    Application.Quit();
                    break;
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
        for (transitionTimer = 0; transitionTimer < transitionDuration; transitionTimer += Time.unscaledDeltaTime)
        {
            float f = transitionTimer / transitionDuration;
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
