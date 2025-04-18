using UnityEngine;
using UnityEngine.EventSystems;

public class DocumentMiniature : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Transform docContainer;
    [SerializeField] private ScreenRoot docViewerScreenPrefab;
    [SerializeField] private bool moveToFrontOnHover;
    [SerializeField] private DocumentConfig documentConfig;
    [SerializeField] private TMPro.TextMeshProUGUI shortTitleText;
    [SerializeField] private TMPro.TextMeshProUGUI fullTitleText;
    [SerializeField] private GameObject fullTitleTooltip;
    [SerializeField] private GameObject newIndicator;
    //[SerializeField] private GameObject placeholderPrefab;
    [SerializeField] private Transform tooltipContainer;
    public DocumentConfig DocumentConfig => documentConfig;
    private int childIndex;
    
    void Awake()
    {
        shortTitleText.text = documentConfig.shortTitle;
        fullTitleText.text = documentConfig.fullTitle;
        fullTitleTooltip.gameObject.SetActive(false);
        childIndex = transform.GetSiblingIndex();
        float yOffset = fullTitleTooltip.transform.position.y - transform.position.y;
        fullTitleTooltip.transform.rotation = Quaternion.identity;
        fullTitleTooltip.transform.SetParent(tooltipContainer);
        fullTitleTooltip.transform.position = this.transform.position + yOffset * Vector3.up;
        RectTransform docPreview = Instantiate(DocumentConfig.document_prefab, docContainer);
        docPreview.transform.localScale = Vector3.one;
        docPreview.anchoredPosition = Vector2.zero;
    }

    public void OpenDocument()
    {
        ScreenRoot docViewer = ScreenTransitionManager.instance.InstantiateScreen(docViewerScreenPrefab, ScreenStackMode.Push);

        docViewer.GetComponentInChildren<DocViewer>().OpenDocument(documentConfig.id);
    }

    public void SetNew(bool isNew)
    {
        newIndicator.SetActive(isNew);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("enter");
        fullTitleTooltip.gameObject.SetActive(true);
        if(moveToFrontOnHover) transform.SetAsLastSibling();
        fullTitleTooltip.transform.SetAsFirstSibling();
        SetNew(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        fullTitleTooltip.gameObject.SetActive(false);
        transform.SetSiblingIndex(childIndex);
        //Debug.Log("child index = " + transform.GetSiblingIndex());
    }
}
