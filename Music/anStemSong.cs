using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class anStemSong : IanSong
{
    anStemMusicMag Mag;
    AudioMixerGroup Output;
    public List<anSourcerer> SourceHandlers = new List<anSourcerer>();//need sourcerer??
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
        foreach (anSourcerer s in SourceHandlers)
            s.audioSource.PlayScheduled(startTime);
        anCore.PlayClipScheduled(transform, Mag.Impact.Clip, Mag.Impact.Gain, startTime, Output, Mag.OneShotPrefab);
    }
    public override void StopOnCue(double stopTime)
    {
        foreach (anSourcerer s in SourceHandlers)
        {
            AudioSource a = s.audioSource;
            a.SetScheduledEndTime(stopTime);
            s.StartCoroutine(anCore.DeleteWhenDone(a, stopTime));
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
        StemData[] Stems = Mag.Stems;
        foreach(StemData data in Stems)
        {
            anSourcerer s = Instantiate(Mag.LoopPrefab, transform);
            AudioSource a = s.audioSource;
            a.clip = data.Clip;
            a.panStereo = data.Pan;
            a.volume = 1;
            a.outputAudioMixerGroup = data.Channel;
            SourceHandlers.Add(s);
        }
        Output = output;
    }
    public override void StopImmediate()
    {
        Destroy(gameObject);
        anSynchro.StopSynchro();
    }
    public override void Mute(bool toMute)
    {
        foreach (anSourcerer s in SourceHandlers)
            s.audioSource.mute = toMute;
    }
}