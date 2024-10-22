using UnityEngine;
using UnityEngine.UI;

public class MapButton : MonoBehaviour
{
    public MapPointConfig config;
    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
        ScreenTransitionManager map_screen = GetComponentInParent<ScreenTransitionManager>();

        button.onClick.AddListener(() =>
        {
        });
    }

    void Update()
    {
        
    }
}
