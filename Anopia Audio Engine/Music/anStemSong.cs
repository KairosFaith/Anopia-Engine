using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class anStemSong : IanSong
{
    anStemMusicMag Mag;
    public List<anSourcerer> SourceHandlers = new List<anSourcerer>();//need sourcerer??
    public override void FadeOut(float t, Action ondone = null)
    {
        Action<float> ChangeValue = null;
        foreach (anSourcerer s in SourceHandlers)
            ChangeValue += (float v) =>
                s.audioSource.volume = v;
        ondone += () =>
        {
            Destroy(gameObject);
            anSynchro.StopSynchro();
        };
        StartCoroutine(anCore.FadeValue(t, 1, 0, ChangeValue, ondone));
    }
    public override void Play(double startTime)
    {
        foreach (anSourcerer s in SourceHandlers)
            s.audioSource.PlayScheduled(startTime);
    }
    public override void StopCue(double stopTime)
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
    public void Setup(MonoBehaviour host, IanMusicMag mag, AudioMixerGroup[] busses)
    {
        Mag = (anStemMusicMag)mag;
        Dictionary<AudioMixerGroup, StemData> stems = Mag.GetStems();
        foreach(KeyValuePair<AudioMixerGroup, StemData> p in stems)
        {
            anSourcerer s = Instantiate(Mag.LoopPrefab, transform).GetComponent<anSourcerer>();
            AudioSource a = s.audioSource;
            StemData data = p.Value;
            a.clip = data.Clip;
            a.panStereo = data.Pan;
            a.volume = 1;
            a.loop = true;
            SourceHandlers.Add(s);
        }
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