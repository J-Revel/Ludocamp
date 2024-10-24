using System;
using System.Collections;
using UnityEngine;

public class ScreenRoot : MonoBehaviour
{
    public delegate IEnumerator ScreenTransitionDelegate();

    public IEnumerator disappear_coroutine { 
        get
        {
            return GetComponent<IScreenDisappearTransition>().DisappearTransitionCoroutine();
        }
    }
    public IEnumerator appear_coroutine { 
        get
        {
            return GetComponent<IScreenAppearTransition>().AppearTransitionCoroutine();
        }
    }
}

public interface IScreenAppearTransition
{
    public IEnumerator AppearTransitionCoroutine();
}

public interface IScreenDisappearTransition
{
    public IEnumerator DisappearTransitionCoroutine();
}
