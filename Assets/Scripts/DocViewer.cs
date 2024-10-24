using UnityEngine;
using System.Collections.Generic;

public class DocViewer : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI docTitleText;
    [SerializeField] private UnityEngine.UI.ScrollRect scrollView;
    [SerializeField] private Transform documentContainer;
    [SerializeField] private AudioSource openDocSound;
    [SerializeField] private AudioSource moveDocSound;
    [SerializeField] private UnityEngine.UI.ScrollRect scrollRect;
    private float scrollDirection;
    private float lastScrollPosition;
    private bool isPlayingSlideSound;
    private float lastScrollDir;
    private float lastScrollPlaySoundTime;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        scrollView.verticalNormalizedPosition = 1f;
        //scrollView.onValueChanged.AddListener(x => moveDocSound.Play());
    }

    private void Update()
    {
        //scrollRect.verticalNormalizedPosition += Time.deltaTime * scrollDirection * scrollSpeed;
        float scrollDir = 0;

        if(scrollRect.verticalNormalizedPosition - lastScrollPosition > Mathf.Epsilon)
        {
            scrollDir = 1f;
        }
        else if (scrollRect.verticalNormalizedPosition - lastScrollPosition < -Mathf.Epsilon)
        {
            scrollDir = -1f;
        }

        if(scrollDir != 0)
        {
            if(lastScrollDir == 0 && Time.time - lastScrollPlaySoundTime > 1.0f) // || lastScrollDir * scrollDir < -Mathf.Epsilon)
            {
                lastScrollPlaySoundTime = Time.time;
                moveDocSound.Play();
            }
        }

        lastScrollDir = scrollDir;

        //if (Mathf.Abs(scrollRect.verticalNormalizedPosition - lastScrollPosition) < Mathf.Epsilon)
        //{
        //    moveDocSound.Stop();
        //    isPlayingSlideSound = false;
        //}
        //else if (!isPlayingSlideSound)
        //{
        //    moveDocSound.Play();
        //    isPlayingSlideSound = true;
        //}
        lastScrollPosition = scrollRect.verticalNormalizedPosition;


        // close screen if on top
        if (Input.GetKeyDown(KeyCode.Escape) && ScreenTransitionManager.instance.IsTopScreen(GetComponent<ScreenRoot>()))
        {
            ScreenTransitionManager.instance.CloseScreen();
        }
    }

    public void OpenDocument(DocumentConfig docConfig)
    {
        EvaluationReport.Instance.AddViewedDoc(docConfig);

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

        docTitleText.text = docConfig.fullTitle;

        RectTransform docInstance = Instantiate(docConfig.document_prefab, documentContainer);
        docInstance.anchoredPosition = Vector3.zero;
        //Instantiate(docConfig.document_prefab, documentContainer);

        openDocSound.Play();
    }
}
