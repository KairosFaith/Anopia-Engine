using UnityEngine;
using System;
using System.Collections;
public class anSourcerer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioDistortionFilter audioDistortionFilter;
    public AudioHighPassFilter audioHighPassFilter;
    public void DeleteWhenDone(Action onDone = null)
    {
        if(audioSource.isPlaying)
            StartCoroutine(_DeleteWhenDone( onDone));
        else
        {
            onDone?.Invoke();
            Destroy(gameObject);
        }
    }
    IEnumerator _DeleteWhenDone(Action onDone)
    {
        while (audioSource.isPlaying)
            yield return new WaitForEndOfFrame();
        onDone?.Invoke();
        Destroy(gameObject);
    }
    public void DeleteAfterTime( double stopTime, Action onDone = null)
    {
        StartCoroutine(_DeleteAfterTime(stopTime, onDone));
    }
    IEnumerator _DeleteAfterTime(double stopTime, Action onDone)
    {
        double curTime = AudioSettings.dspTime;
        while (curTime < stopTime)
            yield return new WaitForSeconds((float)(stopTime-curTime));
        StartCoroutine(_DeleteWhenDone(onDone));
    }
}