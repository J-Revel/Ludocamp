using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DocViewer : MonoBehaviour
{
    public DocumentConfig[] documents;
    [SerializeField] private TMPro.TextMeshProUGUI docTitleText;
    [SerializeField] private UnityEngine.UI.ScrollRect scrollView;
    [SerializeField] private Transform documentContainer;
    [SerializeField] private AudioSource openDocSound;
    [SerializeField] private AudioSource moveDocSound;
    [SerializeField] private AudioSource slideSound;
    [SerializeField] private UnityEngine.UI.ScrollRect scrollRect;
    [SerializeField] private UnityEngine.UI.Button upArrow, downArrow;
    private float scrollDirection;
    private float lastScrollPosition;
    private bool isPlayingSlideSound;
    private float lastScrollDir;
    private float lastScrollPlaySoundTime;
    private bool isSliding;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        scrollView.verticalNormalizedPosition = 1f;
        //scrollView.onValueChanged.AddListener(x => moveDocSound.Play());
    }

    private void Update()
    {
        
        //float scrollDir = 0;

        //if(scrollRect.verticalNormalizedPosition - lastScrollPosition > Mathf.Epsilon)
        //{
        //    scrollDir = 1f;
        //}
        //else if (scrollRect.verticalNormalizedPosition - lastScrollPosition < -Mathf.Epsilon)
        //{
        //    scrollDir = -1f;
        //}

        //if(scrollDir != 0)
        //{
        //    if(lastScrollDir == 0 && Time.time - lastScrollPlaySoundTime > 1.0f) // || lastScrollDir * scrollDir < -Mathf.Epsilon)
        //    {
        //        lastScrollPlaySoundTime = Time.time;
        //        moveDocSound.Play();
        //    }
        //}

        if (Mathf.Abs(scrollRect.verticalNormalizedPosition - lastScrollPosition) < Mathf.Epsilon)
        {
            slideSound.Stop();
            isPlayingSlideSound = false;
        }
        else if (!isPlayingSlideSound)
        {
            slideSound.Play();
            isPlayingSlideSound = true;
        }
        lastScrollPosition = scrollRect.verticalNormalizedPosition;

        upArrow.interactable = scrollRect.verticalNormalizedPosition < 0.99f;
        downArrow.interactable = scrollRect.verticalNormalizedPosition > 0.01f;

        //lastScrollDir = scrollDir;

        //lastScrollPosition = scrollRect.verticalNormalizedPosition;

        // close screen if on top
        if (Input.GetKeyDown(KeyCode.Escape) && ScreenTransitionManager.instance.IsTopScreen(GetComponent<ScreenRoot>()))
        {
            ScreenTransitionManager.instance.CloseScreen();
        }
    }

    private IEnumerator MoveToPosition(float normalizedPosition)
    {
        slideSound.Play();
        isSliding = true;
        scrollRect.GetComponent<CanvasGroup>().interactable = false;
        float startPos = scrollRect.verticalNormalizedPosition;
        float duration = 0.5f;
        for (float timer = 0; timer < duration; timer += Time.unscaledDeltaTime)
        {
            float f = timer / duration;
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(startPos, normalizedPosition, f);
            yield return null;
        }
        scrollRect.GetComponent<CanvasGroup>().interactable = true;
        isSliding = false;

    }

    public void OnUpArrowClicked()
    {
        if (isSliding) return;
        StartCoroutine(MoveToPosition(1f));
    }

    public void OnDownArrowClicked()
    {
        if (isSliding) return;
        StartCoroutine(MoveToPosition(0f));
    }

    public void OpenDocument(string document_id)
    {
        EvaluationReport.Instance.RemoveNotViewedDoc(document_id);
        DocumentConfig selected_document = null;
        foreach (var document in documents)
        {
            if (document.id == document_id)
            {
                selected_document = document;
            }
        }

        if (selected_document == null)
            return;

        List<GameObject> toDelete = new List<GameObject>();

        for (int i = 0; i < documentContainer.childCount; i++)
        {
            //Debug.Log("record todelete");
            toDelete.Add(documentContainer.GetChild(i).gameObject);
        }

        foreach (var go in toDelete)
        {
            //Debug.Log("destroy " + go.name);
            Destroy(go);
        }

        //Debug.Log("open doc");

        docTitleText.text = selected_document.fullTitle;

        RectTransform docInstance = Instantiate(selected_document.document_prefab, documentContainer);
        docInstance.anchoredPosition = Vector3.zero;
        //Instantiate(docConfig.document_prefab, documentContainer);

        openDocSound.Play();
    }
}
