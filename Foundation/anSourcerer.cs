using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Audio;
[RequireComponent(typeof(AudioSource))]
public class anSourcerer : MonoBehaviour
{
    public AudioSource audioSource
    {
        get 
        { 
            if (_audioSource == null)
                _audioSource = this.GetOrAddComponent<AudioSource>();
            return _audioSource; 
        } 
    }
    public AudioDistortionFilter audioDistortionFilter
    {
        get
        {
            if (_audioDistortionFilter == null)
                _audioDistortionFilter = this.GetOrAddComponent<AudioDistortionFilter>();
            return _audioDistortionFilter;
        }
    }
    public AudioHighPassFilter audioHighPassFilter
    {
        get
        {
            if (_audioHighPassFilter == null)
                _audioHighPassFilter = this.GetOrAddComponent<AudioHighPassFilter>();
            return _audioHighPassFilter;
        }
    }
    AudioSource _audioSource;
    AudioDistortionFilter _audioDistortionFilter;
    AudioHighPassFilter _audioHighPassFilter;
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
    public void SetChannel(AudioMixerGroup channel)
    {
        audioSource.outputAudioMixerGroup = channel;
    }
    public void SetRandomPitch(float minPitch = 1, float maxPitch = 1)
    {
        audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
    }
    public void PlayScheduled(double timecode, float gain)
    {
        audioSource.volume = gain;
        audioSource.PlayScheduled(timecode);
    }
}