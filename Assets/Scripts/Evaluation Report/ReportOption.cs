using UnityEngine;
using UnityEngine.UI;

public class ReportOption : MonoBehaviour
{
    [SerializeField] private AudioSource clickAudio;
    [SerializeField] private int correctOption;
    [SerializeField] private ReportOptionConfig optionConfig;
    [SerializeField] private TMPro.TMP_Dropdown optionDropdown;

    public event System.Action<ReportOption> Validated;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        UpdateOptions();

        optionDropdown.onValueChanged.RemoveAllListeners();

        optionDropdown.onValueChanged.AddListener(OnValueChanged);

        //Debug.Log($"start {gameObject.name}");
    }

    private void OnValueChanged(int value)
    {
        clickAudio.Play();
        if (value == correctOption)
        {
            Validated?.Invoke(this);
        }
    }

    public bool IsOptionValid()
    {
        return optionDropdown.value == correctOption;
    }

    private void UpdateOptions()
    {
        optionDropdown.options.Clear();

        foreach(var option in optionConfig.options)
        {
            optionDropdown.options.Add(new TMPro.TMP_Dropdown.OptionData(option));
        }    
    }
}
