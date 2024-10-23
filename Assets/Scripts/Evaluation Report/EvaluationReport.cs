using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EvaluationReport : MonoBehaviour
{
    [SerializeField] private RectTransform pagesContainer;
    
    private int currentBlockIndex;
    private ReportBlock currentBlock;
    private ReportBlock[] blocks;
    public event System.Action<DocumentConfig> DocumentUnlocked;
    public event System.Action<DialogueConfig> DialogueUnlocked;
    public ReportPage[] pages;
    public int TotalPage => pages.Length;
    private int currentPageIndex;
    public static EvaluationReport Instance;
    private HashSet<DocumentConfig> unlockedDocuments; // { get; private set; }
    private HashSet<DialogueConfig> unlockedDialogues; // { get; private set; }
    private HashSet<DocumentConfig> viewedDocuments; // { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        Instance = this;
        unlockedDocuments = new HashSet<DocumentConfig>();
        unlockedDialogues = new HashSet<DialogueConfig>();
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

        SetCurrentBlock(0);
    }

    public bool IsDialogueUnlocked(DialogueConfig dialogue) => unlockedDialogues.Contains(dialogue);
    public bool IsDocumentUnlocked(DocumentConfig doc) => unlockedDocuments.Contains(doc);
    public List<DialogueConfig> UnlockedDialogues => unlockedDialogues.ToList();
    public List<DocumentConfig> UnlockedDocuments => unlockedDocuments.ToList();
    public List<DocumentConfig> ViewedDocuments => viewedDocuments.ToList();
    public void AddViewedDoc(DocumentConfig doc) => viewedDocuments.Add(doc);
    public void UnlockDocument(DocumentConfig docConfig)
    {
        unlockedDocuments.Add(docConfig);
        DocumentUnlocked?.Invoke(docConfig);
        
    }
    public void UnlockDialogue(DialogueConfig dialConfig)
    {
        Debug.Log($"unlock dialogue {dialConfig}");
        unlockedDialogues.Add(dialConfig);
        DialogueUnlocked?.Invoke(dialConfig);
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
        SetCurrentBlock(currentBlockIndex + 1);
    }
}
