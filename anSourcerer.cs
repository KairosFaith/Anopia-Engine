using UnityEngine;
using System;
using System.Collections;
[RequireComponent(typeof(AudioSource))]
public class anSourcerer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioDistortionFilter audioDistortionFilter;
    public AudioHighPassFilter audioHighPassFilter;
    public void DeleteWhenDone(Action onDone = null)
    {
        StartCoroutine(_DeleteWhenDone( onDone));
    }
    IEnumerator _DeleteWhenDone(Action onDone)
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        while (audioSource.isPlaying && !AudioListener.pause)
            yield return new WaitForEndOfFrame();
        onDone?.Invoke();
        Destroy(gameObject);
    }
    public void DeleteAfterTime(AudioSource source, double stopTime, Action onDone = null)
    {
        StartCoroutine(_DeleteAfterTime(stopTime, onDone));
    }
    IEnumerator _DeleteAfterTime(double stopTime, Action onDone)
    {
        while ((AudioSettings.dspTime < stopTime) || audioSource.isPlaying)
            yield return new WaitForEndOfFrame();
        onDone?.Invoke();
        UnityEngine.Object.Destroy(gameObject);
    }
}