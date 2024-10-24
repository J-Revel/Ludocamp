using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class EvaluationReport : MonoBehaviour
{
    [SerializeField] private ScreenRoot gameEndScreen;
    [SerializeField] private float initialScroll;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private UnityEngine.UI.ScrollRect scrollRect;
    [SerializeField] private RectTransform pagesContainer;
    
    private int currentBlockIndex;
    private ReportBlock currentBlock;
    private ReportBlock[] blocks;
    public event System.Action<string> DocumentOrDialogueUnlocked;
    //public event System.Action<DialogueConfig> DialogueUnlocked;
    public ReportPage[] pages;
    public int TotalPage => pages.Length;
    private int currentPageIndex;
    public static EvaluationReport Instance;
    private HashSet<string> unlockedConfigIDs;
    //private HashSet<DocumentConfig> unlockedDocuments; // { get; private set; }
    //private HashSet<DialogueConfig> unlockedDialogues; // { get; private set; }
    private HashSet<DocumentConfig> viewedDocuments; // { get; private set; }
 
    public float scrollDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        Instance = this;
        unlockedConfigIDs = new HashSet<string>();
        //unlockedDocuments = new HashSet<DocumentConfig>();
        //unlockedDialogues = new HashSet<DialogueConfig>();
        viewedDocuments = new HashSet<DocumentConfig>();
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        blocks = GetComponentsInChildren<ReportBlock>(includeInactive: true);
        pages = GetComponentsInChildren<ReportPage>(includeInactive: true);
        int pageIndex = 0;
        foreach (var page in pages)
        {
            page.Index = pageIndex;

            foreach (var block in page.GetComponentsInChildren<ReportBlock>(includeInactive: true))
            {
                block.PageIndex = pageIndex;
            }

            page.transform.SetAsFirstSibling();

            pageIndex++;
        }
        scrollRect.verticalNormalizedPosition = initialScroll;
        SetCurrentBlock(0);
    }

    public bool IsDialogueUnlocked(DialogueConfig dialogue) => unlockedConfigIDs.Contains(dialogue.id);
    public bool IsDocumentUnlocked(DocumentConfig doc) => unlockedConfigIDs.Contains(doc.id);
    //public List<DialogueConfig> UnlockedDialogues => unlockedDialogues.ToList();
    //public List<DocumentConfig> UnlockedDocuments => unlockedDocuments.ToList();
    public List<DocumentConfig> ViewedDocuments => viewedDocuments.ToList();
    public void AddViewedDoc(DocumentConfig doc) => viewedDocuments.Add(doc);
    public void UnlockDialogueOrDocument(string id)
    {
        unlockedConfigIDs.Add(id);
        DocumentOrDialogueUnlocked?.Invoke(id);
    }
    public void UnlockDocument(DocumentConfig docConfig)
    {
        unlockedConfigIDs.Add(docConfig.id);
        DocumentOrDialogueUnlocked?.Invoke(docConfig.id);  
    }
    public void UnlockDialogue(DialogueConfig dialConfig)
    {
        //Debug.Log($"unlock dialogue {dialConfig}");
        unlockedConfigIDs.Add(dialConfig.id);
        DocumentOrDialogueUnlocked?.Invoke(dialConfig.id);
    }

    public void OpenPage(int index)
    {
        currentPageIndex = index;

        foreach (var page in pages)
        {
            page.gameObject.SetActive(page.Index >= index);
            if (page.Index > index) page.SetPageColorToBase();
        }

        pages[index].OnPageOpened(pages.Length);
    }

    private void Update()
    {
        scrollRect.verticalNormalizedPosition += Time.deltaTime * scrollDirection * scrollSpeed; 
    }

    public void OnUpArrowClicked()
    {
        scrollDirection = -1;
    }

    public void OnDownArrowClicked()
    {
        scrollDirection = +1;
    }

    public void OnArrowButtonUp()
    {
        scrollDirection = 0;
    }

    private void SetCurrentBlock(int index)
    {
        currentBlockIndex = index;
        currentBlock = blocks[currentBlockIndex];

        for (int i = 0; i < blocks.Length; i++)
        {
            if (i < currentBlockIndex)
            {
                blocks[i].SetValidated();
            }
            else if (i > currentBlockIndex)
            {
                blocks[i].SetDisabled();
            }
            else // equal to currentBlockIndex
            {
                blocks[i].SetCurrent();
                blocks[i].Validated += OnBlockValidated;
            }
        }

        OpenPage(currentBlock.PageIndex);
    }

    private IEnumerator LastBlockValidated()
    {
        currentBlock.SetValidated();
        yield return new WaitForSecondsRealtime(1f);

        ScreenRoot gameEndScreenInstance = ScreenTransitionManager.instance.InstantiateScreen(gameEndScreen, ScreenStackMode.Push);
        gameEndScreenInstance.GetComponentInChildren<SlideViewer>().LastSlideFinished += () => Application.Quit();
    }

    private void OnBlockValidated(ReportBlock block)
    {
        foreach (var unlocked in block.UnlockOnValidate)
        {
            if (unlocked is DocumentConfig docConfig)
            {
                UnlockDocument(docConfig);
                //Debug.Log($"unlocked {docConfig.name}");
            }
            else if (unlocked is DialogueConfig dialogueConfig)
            {
                UnlockDialogue(dialogueConfig);
                //Debug.Log($"unlocked {dialogueConfig.name}");
            }
        }

        block.Validated -= OnBlockValidated;

        if(currentBlockIndex >= blocks.Length - 1) // last block completed
        {
            StartCoroutine(LastBlockValidated());
            return;
        }

        SetCurrentBlock(currentBlockIndex + 1);
    }
}
