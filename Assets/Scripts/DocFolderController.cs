using UnityEngine;
using System.Collections.Generic;

public class DocFolderController : MonoBehaviour
{
    private Dictionary<DocumentConfig, DocumentMiniature> docs;

    private void Start()
    {
        EvaluationReport.Instance.DocumentUnlocked += OnDocUnlocked;
        Initialize();
    }

    private void OnDisable()
    {
        EvaluationReport.Instance.DocumentUnlocked -= OnDocUnlocked;
    }

    private void OnDocUnlocked(DocumentConfig docConfig)
    {
        docs[docConfig].gameObject.SetActive(true);
        docs[docConfig].SetNew(true);
    }

    private void Initialize()
    {
        var unlockedDocs = EvaluationReport.Instance.UnlockedDocuments;
        var viewedDocs = EvaluationReport.Instance.ViewedDocuments;
        docs = new Dictionary<DocumentConfig, DocumentMiniature>();
        //docs = GetComponentsInChildren<DocumentMiniature>(true);
        foreach (var docMiniature in GetComponentsInChildren<DocumentMiniature>(true))
        {
            docs.Add(docMiniature.DocumentConfig, docMiniature);

            if(unlockedDocs.Contains(docMiniature.DocumentConfig))
            {
                docMiniature.gameObject.SetActive(true);
                docMiniature.SetNew(!viewedDocs.Contains(docMiniature.DocumentConfig));
            }
            else
            {
                docMiniature.gameObject.SetActive(false);
            }
        }
    }
}
