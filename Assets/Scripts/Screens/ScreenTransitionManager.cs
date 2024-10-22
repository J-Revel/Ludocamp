using UnityEngine;
using UnityEngine.UI;

public class ScreenTransitionManager : MonoBehaviour
{
    public static ScreenTransitionManager instance;

    void Awake()
    {
        instance = this;
    }

    public void ShowScreen(ScreenRoot prefab)
    {
        Instantiate(prefab);
    }
}
