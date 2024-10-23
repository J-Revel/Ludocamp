using UnityEngine;

public class ReportPage : MonoBehaviour
{
    public int Index { get; set; }
    [SerializeField] private TMPro.TextMeshProUGUI pageNumberText;
    [SerializeField] private UnityEngine.UI.Button previousPageButton, nextPageButton;

    public void OnPageOpened(int totalPage)
    {
        pageNumberText.text = $"{ Index + 1 } / { totalPage }";

        previousPageButton.gameObject.SetActive(Index > 0);
        nextPageButton.gameObject.SetActive(Index < totalPage);
    }

    public void OnNextPageArrowClicked()
    {
        if (Index >= EvaluationReport.Instance.TotalPage - 1) return;

        EvaluationReport.Instance.OpenPage(Index + 1);
    }

    public void OnPreviousPageArrowClicked()
    {
        if (Index <= 0) return;

        EvaluationReport.Instance.OpenPage(Index - 1);
    }
}
