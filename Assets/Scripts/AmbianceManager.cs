using UnityEngine;
using System.Collections;

public class AmbianceManager : MonoBehaviour
{
    //[SerializeField] private AudioClip TEST_ambiance2;
    [SerializeField] private AudioClip baseAmbiance;
    [SerializeField] private AudioSource audioSource1, audioSource2;
    private AudioSource primaryAudioSource, secondaryAudioSource;
    public static AmbianceManager Instance;

    private void Awake()
    {
        Instance = this;
        primaryAudioSource = audioSource1;
        secondaryAudioSource = audioSource2;
        //Debug.Log($"{gameObject.name}, prim = {primaryAudioSource}, sec = {secondaryAudioSource}");
    }

    private void Start()
    {
        ResetToBaseAmbiance();
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        //Debug.Log($"1");
    //        ChangeAmbiance(TEST_ambiance2);
    //    }
    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        //Debug.Log($"1");
    //        ResetToBaseAmbiance();
    //    }
    //}

    public void ChangeAmbiance(AudioClip ambianceClip)
    {
        CrossfadeToAmbiance(ambianceClip);
    }

    public void ResetToBaseAmbiance()
    {
        CrossfadeToAmbiance(baseAmbiance);
    }

    private void CrossfadeToAmbiance(AudioClip ambianceClip)
    {
        StartCoroutine(CrossFadeRoutine(ambianceClip));
    }

    private IEnumerator CrossFadeRoutine(AudioClip ambianceClip)
    {
        //Debug.Log($"crossfade to {ambianceClip}");
        primaryAudioSource.clip = ambianceClip;
        primaryAudioSource.time = 0f;
        primaryAudioSource.Play();
        float duration = 1f;
        for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
        {
            float a = t / duration;
            primaryAudioSource.volume = a;
            secondaryAudioSource.volume = 1 - a;
            yield return null;
        }
        primaryAudioSource.volume = 1;
        secondaryAudioSource.volume = 0;
        var prevPrimary = primaryAudioSource;
        primaryAudioSource = secondaryAudioSource;
        secondaryAudioSource = prevPrimary;
    }
}
