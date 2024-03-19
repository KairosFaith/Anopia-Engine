using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class anStemSong : IanSong
{
    anStemMusicMag Mag;
    AudioMixerGroup Output;
    public anSourcerer[] SourceHandlers;
    public override void FadeOut(float t, Action ondone = null)
    {
        Action<float> ChangeValue = null;
        foreach (anSourcerer s in SourceHandlers)
            ChangeValue += (float v) =>
                s.audioSource.volume = v;
        ondone += () => Destroy(gameObject);
        StartCoroutine(anCore.FadeValue(t, 1, 0, ChangeValue, ondone));
    }
    public override void Play(double startTime)
    {
        SourceHandlers = Mag.Play(startTime, Output);
    }
    public override void StopOnCue(double stopTime)
    {
        foreach (anSourcerer s in SourceHandlers)
        {
            AudioSource a = s.audioSource;
            a.SetScheduledEndTime(stopTime);
            s.DeleteAfterTime(stopTime);
        }
        transform.DetachChildren();
        Destroy(gameObject);
    }
    public override void FadeIn(float t)
    {
        Action<float> ChangeValue = null;
        foreach (anSourcerer s in SourceHandlers)
            ChangeValue += (float v) =>
                s.audioSource.volume = v;
        StartCoroutine(anCore.FadeValue(t, 0, 1, ChangeValue));
    }
    public override void Setup(IanMusicMag mag, AudioMixerGroup output)
    {
        Mag = (anStemMusicMag)mag;
        SourceHandlers = Mag.Setup();
        Output = output;
    }
    public override void StopImmediate()
    {
        foreach (anSourcerer s in SourceHandlers)
            s.audioSource.Stop();
        Destroy(gameObject);
    }
    public override void Mute(bool toMute)
    {
        foreach (anSourcerer s in SourceHandlers)
            s.audioSource.mute = toMute;
    }
}