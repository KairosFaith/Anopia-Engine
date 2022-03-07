using UnityEngine;
using System;
using System.Collections;
[RequireComponent(typeof(AudioSource))]
public class anSourcerer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioDistortionFilter audioDistortionFilter;
    public AudioHighPassFilter audioHighPassFilter;
    public void DeleteWhenDone(AudioSource source, Action onDone = null)
    {
        StartCoroutine(_DeleteWhenDone(source, onDone));
    }
    IEnumerator _DeleteWhenDone(AudioSource source, Action onDone)
    {
        yield return new WaitForSeconds(source.clip.length);
        while (source.isPlaying && !AudioListener.pause)
            yield return new WaitForEndOfFrame();
        onDone?.Invoke();
        Destroy(source.gameObject);
    }
    public void DeleteAfterTime(AudioSource source, double stopTime, Action onDone = null)
    {
        StartCoroutine(_DeleteAfterTime(source, stopTime, onDone));
    }
    IEnumerator _DeleteAfterTime(AudioSource source, double stopTime, Action onDone)
    {
        while ((AudioSettings.dspTime < stopTime) || source.isPlaying)
            yield return new WaitForEndOfFrame();
        onDone?.Invoke();
        UnityEngine.Object.Destroy(source.gameObject);
    }
}