using UnityEngine;
using System.Linq;

public class ReportBlock : MonoBehaviour
{
    [SerializeField] private UnlockableConfig[] unlockOnValidate;
    [SerializeField] private GameObject validatedFrame;
    [SerializeField] private float disabledBlockOpacity;
    [SerializeField] private CanvasGroup canvasGroup;
    public event System.Action<ReportBlock> Validated;
    public UnlockableConfig[] UnlockOnValidate => unlockOnValidate;
    private ReportOption[] options;

    private void Awake()
    {
        options = GetComponentsInChildren<ReportOption>();

        foreach(var option in options)
        {
            option.Validated += OnOptionValidated;
        }

        validatedFrame.transform.SetAsLastSibling();
    }

    public void SetCurrent()
    {
        validatedFrame.SetActive(false);
        canvasGroup.alpha = 1f;
        
    }

    public void SetDisabled()
    {
        validatedFrame.SetActive(false);
        canvasGroup.alpha = disabledBlockOpacity;
    }

    public void SetValidated()
    {
        validatedFrame.SetActive(true);
        foreach(var unlockable in unlockOnValidate)
        {
            unlockable.OnUnlock();
        }
        //validatedFrame.transform.SetAsLastSibling(); // = validatedFrame.transform.parent.childCount;
        Validated?.Invoke(this);
    }

    private void OnOptionValidated(ReportOption option)
    {
        if(IsBlockValid())
        {
            SetValidated();
        }
    }

    public bool IsBlockValid()
    {
        return GetComponentsInChildren<ReportOption>().All(x => x.IsOptionValid());
    }

    private void OnDisable()
    {
        foreach (var option in options)
        {
            option.Validated -= OnOptionValidated;
        }
    }
}
