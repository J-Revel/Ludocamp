using UnityEngine;

public class MapButtonController : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image postitImage;
    [SerializeField] private UnityEngine.UI.Button button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EvaluationReport.Instance.DocumentOrDialogueUnlocked += s => UpdateState();
        EvaluationReport.Instance.DialogueViewed += UpdateState;

        UpdateState();
    }

    void UpdateState()
    {
        button.interactable = EvaluationReport.Instance.IsAnyDialogueUnlocked();
        postitImage.enabled = EvaluationReport.Instance.NotViewedDialogues.Count > 0;
    }
}
