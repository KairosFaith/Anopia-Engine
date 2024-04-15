using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Audio;
[RequireComponent(typeof(AudioSource))]
public class anSourcerer : MonoBehaviour
{
    public AudioMixerGroup Channel
    {
        get { return audioSource.outputAudioMixerGroup; }
        set { audioSource.outputAudioMixerGroup = value; }
    }
    public float Volume
    {
        get { return audioSource.volume; }
        set { audioSource.volume = value; }
    }
    public float Pitch
    {
        get { return audioSource.pitch; }
        set { audioSource.pitch = value; }
    }
    public float Distortion
    {
        get { return audioDistortionFilter.distortionLevel; }
        set { audioDistortionFilter.distortionLevel = value; }
    }
    public float HighPass
    {
        get { return audioHighPassFilter.cutoffFrequency; }
        set { audioHighPassFilter.cutoffFrequency = value; }
    }
    public AudioSource audioSource
    {
        get 
        { 
            if (_audioSource == null)
                _audioSource = GetComponent<AudioSource>();
            return _audioSource; 
        } 
    }
    public AudioDistortionFilter audioDistortionFilter
    {
        get
        {
            if (_audioDistortionFilter == null)
            {
                _audioDistortionFilter = GetComponent<AudioDistortionFilter>();
                if (_audioDistortionFilter == null)
                    _audioDistortionFilter = gameObject.AddComponent<AudioDistortionFilter>();
            }
            return _audioDistortionFilter;
        }
    }
    public AudioHighPassFilter audioHighPassFilter
    {
        get
        {
            if (_audioHighPassFilter == null)
                _audioHighPassFilter = GetComponent<AudioHighPassFilter>();
            return _audioHighPassFilter;
        }
    }
    AudioSource _audioSource;
    AudioDistortionFilter _audioDistortionFilter;
    AudioHighPassFilter _audioHighPassFilter;
    public void SetUp(anTrackData track, bool loop)
    {
        SetUp(track.Clip, track.Channel, loop);
        audioSource.panStereo = track.Pan;
    }
    public void SetUp(AudioClip clip, AudioMixerGroup channel, bool loop)
    {
        Channel = channel;
        audioSource.clip = clip;
        audioSource.loop = loop;
    }
    public void SetRandomPitch(float minPitch = 1, float maxPitch = 1)
    {
        Pitch = UnityEngine.Random.Range(minPitch, maxPitch);
    }
    public void PlayScheduled(double timecode, float gain = 1)
    {
        audioSource.volume = gain;
        audioSource.PlayScheduled(timecode);
    }
    public void StopScheduled(double timecode)
    {
        audioSource.SetScheduledEndTime(timecode);
        DeleteAfterTime(timecode);
    }
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
    /// <summary>
    /// waits for a certain time, then deletes the gameobject after it has finished playing
    /// </summary>
    /// <param name="stopTime"></param>
    /// <param name="onDone"></param>
    public void DeleteAfterTime( double stopTime, Action onDone = null)
    {
        StartCoroutine(_DeleteAfterTime(stopTime, onDone));
    }
    IEnumerator _DeleteAfterTime(double stopTime, Action onDone)
    {
        double curTime = AudioSettings.dspTime;
        while (curTime < stopTime)
            yield return new WaitForSeconds((float)(stopTime-curTime));
        DeleteWhenDone(onDone);
    }
}