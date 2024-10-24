using UnityEngine;
using System.Collections.Generic;

public class DocViewer : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI docTitleText;
    [SerializeField] private UnityEngine.UI.ScrollRect scrollView;
    [SerializeField] private Transform documentContainer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        scrollView.verticalNormalizedPosition = 1f;
    }

    private void Update()
    {
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

        for(int i = 0; i<documentContainer.childCount;i++)
        {
            //Debug.Log("record todelete");
            toDelete.Add(documentContainer.GetChild(i).gameObject);
        }

        foreach(var go in toDelete)
        {
            //Debug.Log("destroy " + go.name);
            Destroy(go);
        }

        //Debug.Log("open doc");

        docTitleText.text = docConfig.fullTitle;

        RectTransform docInstance = Instantiate(docConfig.document_prefab, documentContainer);
        docInstance.anchoredPosition = Vector3.zero;
        //Instantiate(docConfig.document_prefab, documentContainer);
    }
}
