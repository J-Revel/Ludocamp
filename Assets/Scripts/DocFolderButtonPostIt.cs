using UnityEngine;

public class DocFolderButtonPostIt : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image postitImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EvaluationReport.Instance.DocumentOrDialogueUnlocked += s => UpdateState();
        EvaluationReport.Instance.DocumentViewed += UpdateState;

        UpdateState();
    }

    void UpdateState()
    {
        postitImage.enabled = EvaluationReport.Instance.NotViewedDocuments.Count > 0;
    }
}