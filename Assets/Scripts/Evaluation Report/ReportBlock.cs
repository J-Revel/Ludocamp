using UnityEngine;
using System.Linq;

public class ReportBlock : MonoBehaviour
{
    [SerializeField] private UnlockableConfig[] unlockOnValidate;
    [SerializeField] private AudioSource validateSound;
    [SerializeField] private GameObject validatedFrame;
    [SerializeField] private float disabledBlockOpacity;
    [SerializeField] private CanvasGroup canvasGroup;
    public float validate_scroll_position = 0;
    public event System.Action<ReportBlock> Validated;
    public UnlockableConfig[] UnlockOnValidate => unlockOnValidate;
    private ReportOption[] options;
    public int PageIndex { get; set; }

    private void Awake()
    {
        options = GetComponentsInChildren<ReportOption>(includeInactive: true);

        foreach(var option in options)
        {
            option.Validated += OnOptionValidated;
        }

        validatedFrame.transform.SetAsLastSibling();
    }

	public void SetCurrent()
    {
        validatedFrame.SetActive(false);
        canvasGroup.interactable = true;
        canvasGroup.alpha = 1f;
        
    }

    public void SetDisabled()
    {
        validatedFrame.SetActive(false);
        canvasGroup.alpha = disabledBlockOpacity;
        canvasGroup.interactable = false;
    }

    public void SetValidated()
    {
        canvasGroup.interactable = true;
        validatedFrame.SetActive(true);
        validateSound.Play();
        //foreach(var unlockable in unlockOnValidate)
        //{
        //    unlockable.OnUnlock();
        //}
        //validatedFrame.transform.SetAsLastSibling(); // = validatedFrame.transform.parent.childCount;
        Validated?.Invoke(this);
    }

    private void OnOptionValidated(ReportOption option)
    {
        if(IsBlockValid())
        {
            Debug.Log("option valid");
            SetValidated();
        }
    }

    public bool IsBlockValid()
    {
        return GetComponentsInChildren<ReportOption>().All(x => x.IsOptionValid());
    }

    //private void OnDisable()
    //{
    //    foreach (var option in options)
    //    {
    //        option.Validated -= OnOptionValidated;
    //    }
    //}
}
