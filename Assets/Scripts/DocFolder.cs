using UnityEngine;
using System.Collections.Generic;

public class DocFolder : MonoBehaviour
{
    [SerializeField] private AudioSource openSound;
    private Dictionary<string, DocumentMiniature> docs;

    private void Start()
    {
        EvaluationReport.Instance.DocumentOrDialogueUnlocked += OnDocUnlocked;
        openSound.Play();
        Initialize();
    }

    private void OnDisable()
    {
        EvaluationReport.Instance.DocumentOrDialogueUnlocked -= OnDocUnlocked;
    }

    private void Update()
    {
        // close screen if on top
        if (Input.GetKeyDown(KeyCode.Escape) && ScreenTransitionManager.instance.IsTopScreen(GetComponent<ScreenRoot>()))
        {
            ScreenTransitionManager.instance.CloseScreen();
        }
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
        var notViewedDocs = EvaluationReport.Instance.NotViewedDocuments;
        docs = new Dictionary<string, DocumentMiniature>();
        //docs = GetComponentsInChildren<DocumentMiniature>(true);
        foreach (var docMiniature in GetComponentsInChildren<DocumentMiniature>(true))
        {
            docs.Add(docMiniature.DocumentConfig.id, docMiniature);

            if(EvaluationReport.Instance.IsDocumentUnlocked(docMiniature.DocumentConfig))
            {
                docMiniature.gameObject.SetActive(true);
                docMiniature.SetNew(notViewedDocs.Contains(docMiniature.DocumentConfig.id));
            }
            else
            {
                docMiniature.gameObject.SetActive(false);
            }
        }
    }
}
