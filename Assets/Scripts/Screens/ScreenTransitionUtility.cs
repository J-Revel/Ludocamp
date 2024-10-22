using JetBrains.Annotations;
using UnityEngine;

public class ScreenTransitionUtility : MonoBehaviour
{
    public ScreenRoot target_screen;
    public ScreenStackMode transition_mode;

    public void OpenScreen()
    {
        ScreenTransitionManager.instance.ShowScreen(target_screen, transition_mode);
    }

    public void CloseScreen()
    {
        ScreenTransitionManager.instance.CloseScreen();
    }
}
