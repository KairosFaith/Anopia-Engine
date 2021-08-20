using System;
using UnityEngine;
using UnityEngine.Audio;
public class MusicController : SingletonMonobehavior<MusicController>
{
    public AudioFileID[] MusicGroups;
    SourceGroup CurrentMusicGroup;
    public AudioMixer Mixer;
    public AudioMixerGroup[] Channels;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
    public void SetSnapshot(string Snapshot, float time)
    {
        Mixer.TransitionToSnapshot(Snapshot, time);
    }
    public void ChangeMusic(int key, float time = 0)
    {
        FadeOut(time);
        NewFadeIn(MusicGroups[key], time);
    }
    public void FadeOut(float fadeTime)
    {
        if (CurrentMusicGroup != null)
        {
            CurrentMusicGroup.FadeOut(fadeTime);//add ondone?
            CurrentMusicGroup = null;
        }
    }
    public void NewFadeIn(AudioFileID newMusic, float fadeTime)
    {
        GameObject g = new GameObject();
        g.transform.position = transform.position;
        g.transform.SetParent(transform);

        CurrentMusicGroup = g.AddComponent<SourceGroup>();

        CurrentMusicGroup.InvokeSources(newMusic, Channels);//music use default
        CurrentMusicGroup.FadeIn(fadeTime);
        CurrentMusicGroup.Play();
    }
    public void Interrupt(int clipIndex)
    {
        Action onDone = () =>
        {
            CurrentMusicGroup.Resume();
        };
        Interrupt(clipIndex, onDone);
    }
    public void Interrupt(int clipIndex, Action schedule)
    {
        CurrentMusicGroup.Pause();
        ClipBag mag = FlexEngine.GetClips(MusicGroups[clipIndex]);
        ClipData c = mag.RandomClip();//expect single file
        FlexEngine.PlayClipAtStereo(this, c, Mixer.outputAudioMixerGroup, schedule);
    }
}