using UnityEngine;

public class DocViewerController : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.ScrollRect scrollView;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        scrollView.verticalNormalizedPosition = 1f;
    }
}
