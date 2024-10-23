using UnityEngine;
using System.Collections.Generic;

public class DocFolderController : MonoBehaviour
{
    private Dictionary<string, DocumentMiniature> docs;

    private void Start()
    {
        EvaluationReport.Instance.DocumentOrDialogueUnlocked += OnDocUnlocked;
        Initialize();
    }

    private void OnDisable()
    {
        EvaluationReport.Instance.DocumentOrDialogueUnlocked -= OnDocUnlocked;
    }

    private void OnDocUnlocked(string id)
    {
        if (!docs.ContainsKey(id)) return;
        docs[id].gameObject.SetActive(true);
        docs[id].SetNew(true);
    }

    private void Initialize()
    {
        //var unlockedDocs = EvaluationReport.Instance.UnlockedDocuments;
        var viewedDocs = EvaluationReport.Instance.ViewedDocuments;
        docs = new Dictionary<string, DocumentMiniature>();
        //docs = GetComponentsInChildren<DocumentMiniature>(true);
        foreach (var docMiniature in GetComponentsInChildren<DocumentMiniature>(true))
        {
            docs.Add(docMiniature.DocumentConfig.id, docMiniature);

            if(EvaluationReport.Instance.IsDocumentUnlocked(docMiniature.DocumentConfig))
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
