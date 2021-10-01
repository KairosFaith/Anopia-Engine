using System;
using UnityEngine;
using UnityEngine.Audio;
public class AnopiaMusicController : SingletonMonobehavior<AnopiaMusicController>
{
    AnopiaSourceGroup CurrentMusicGroup;
    public static AudioMixer Mixer;
    public static AudioMixerGroup[] Channels;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
    public static void SetSnapshot(string Snapshot, float time)
    {
        Mixer.TransitionToSnapshot(Snapshot, time);
    }
    public void ChangeMusic(string MusicTag, float time = 0)
    {
        FadeOut(time);
        NewFadeIn(MusicTag, time);
    }
    public void ChangeMusicOnCue(string MusicTag, double transitTime)
    {
        CurrentMusicGroup.StopScheduled(transitTime);
        NewGroup(MusicTag);
        CurrentMusicGroup.PlayScheduled(transitTime);
    }
    public void PlayLayerOnCue(int key, double cueTime)
    {
        CurrentMusicGroup.Sources[key].PlayScheduled(cueTime);
    }
    public void StopLayerOnCue(int key, double cueTime)
    {
        CurrentMusicGroup.Sources[key].SetScheduledEndTime(cueTime);
    }
    public void FadeOut(float fadeTime)
    {
        if (CurrentMusicGroup != null)
        {
            CurrentMusicGroup.FadeOut(fadeTime);//add ondone?
            CurrentMusicGroup = null;
        }
    }
    void NewGroup(string newMusicTag)
    {
        GameObject g = new GameObject();
        g.transform.position = transform.position;
        g.transform.SetParent(transform);

        CurrentMusicGroup = g.AddComponent<AnopiaSourceGroup>();
        CurrentMusicGroup.InvokeSources(newMusicTag, Channels);
    }
    public void NewFadeIn(string newMusicTag, float fadeTime)
    {
        NewGroup(newMusicTag);
        CurrentMusicGroup.FadeIn(fadeTime);
        CurrentMusicGroup.Play();
    }
    public void Interrupt(string MusicTag)
    {
        Action onDone = () =>
        {
            CurrentMusicGroup.Resume();
        };
        Interrupt(MusicTag, onDone);
    }
    void Interrupt(string MusicTag, Action schedule)
    {
        CurrentMusicGroup.Pause();
        anClipMag mag = (anClipMag)AnopiaAudioCore.FetchMag(MusicTag);
        ClipData[] array = mag.Data;
        int r = UnityEngine.Random.Range(0, array.Length);
        ClipData c = array[r];//expect single file
        AnopiaAudioCore.PlayClipAtStereo(this, c, Mixer.outputAudioMixerGroup, 0, schedule);
    }
}