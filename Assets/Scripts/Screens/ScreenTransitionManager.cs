using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum ScreenStackMode
{
    Push, Replace
}

public class ScreenTransitionManager : MonoBehaviour
{
    [SerializeField] private ScreenRoot startScreen;
    public static ScreenTransitionManager instance;
    private List<ScreenRoot> screen_stack = new List<ScreenRoot>();
    private List<Coroutine> transition_coroutines = new List<Coroutine>();

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if(startScreen != null)
        {
            ShowScreen(startScreen, ScreenStackMode.Push);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && screen_stack.Count == 0)
        {
            Application.Quit();
        }
    }

    public bool IsTopScreen(ScreenRoot screen) => screen_stack.Count > 0 && screen_stack.First() == screen;

    public ScreenRoot InstantiateScreen(ScreenRoot prefab, ScreenStackMode screen_stack_mode)
    {
        //ScreenRoot screen = Instantiate(prefab);
        switch(screen_stack_mode)
        {
            case ScreenStackMode.Push:
                break;
            case ScreenStackMode.Replace:
                ScreenRoot to_remove = screen_stack[^1];
                transition_coroutines.Add(StartCoroutine(to_remove.disappear_coroutine));
                screen_stack.RemoveAt(screen_stack.Count - 1);
                break;
        }
        ScreenRoot screen_root = Instantiate(prefab, transform);
        screen_stack.Add(screen_root);
        transition_coroutines.Add(StartCoroutine(screen_root.appear_coroutine));
        return screen_root;
    }

    public void ShowScreen(ScreenRoot prefab, ScreenStackMode screen_stack_mode)
    {
        InstantiateScreen(prefab, screen_stack_mode);
    }

    public void CloseScreen()
    {
        ScreenRoot to_remove = screen_stack[^1];
        transition_coroutines.Add(StartCoroutine(to_remove.disappear_coroutine));
        screen_stack.RemoveAt(screen_stack.Count - 1);
    }
}
