using System;
using UnityEngine;
using UnityEngine.Audio;
public class AnopiaConductor : SingletonMonobehavior<AnopiaConductor>
{
    IanSong CurrentSong;
    public int CurrentBeat;
    public AudioMixerGroup MainChannel;
    public AudioMixer MusicMixer;//for snapshots
    public AudioMixerGroup[] StemChannels;//for using stems
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
    private void Update()
    {
        CurrentBeat = AnopiaSynchro.CurrentBeatCount;
    }
    public IanSong PlayNewSong(string songID)//play immediate
    {
        if (CurrentSong != null)
        {
            CurrentSong.StopImmediate();
            AnopiaSynchro.StopSynchro();
        }
        IanMusicMag mag = (IanMusicMag)AnopiaAudioCore.FetchMag(songID);
        NewSong(mag);
        AnopiaSynchro.StartSynchro(this, mag.Tempo, CurrentSong.Play);
        return CurrentSong;
    }
    public void CueSong(string songID, double timeCode)//user defined cue, assume same tempo
    {
        if (CurrentSong != null)
            CurrentSong.StopCue(timeCode);
        IanMusicMag mag = (IanMusicMag)AnopiaAudioCore.FetchMag(songID);
        NewSong(mag);
        CurrentSong.Play(timeCode);
    }
    public void CueSection(int key)
    {
        if (CurrentSong is anLinearSong ls)
            ls.CueSection(key);
    }
    public void CueFinal()
    {
        if (CurrentSong is anLinearSong ls)
            ls.CueFinal();
    }
    void NewSong(IanMusicMag mag)
    {
        GameObject g = new GameObject();
        g.transform.position = transform.position;
        g.transform.SetParent(transform);
        SongForm songForm = mag.Structure;
        switch (songForm)
        {
            case SongForm.Linear:
                anLinearSong ls = g.AddComponent<anLinearSong>();
                ls.Setup(mag, MainChannel);
                CurrentSong = ls;
                break;
            case SongForm.Stem:
                anStemSong stem = g.AddComponent<anStemSong>();
                stem.Setup(this, mag, StemChannels);
                CurrentSong = stem;
                break;
        }
    }
    public void Stop(double stopTime)//user defined cue
    {
        CurrentSong.StopCue(stopTime);
        AnopiaSynchro.StopSynchro();
    }
    public void Crossfade(float t, string songID)
    {
        FadeOut(t, ()=> FadeIn(t, songID));
    }
    public void FadeOut(float t, Action onDone)
    {
        CurrentSong.FadeOut(t, onDone); 
        AnopiaSynchro.StopSynchro();
    }
    public void FadeIn(float t, string songID)
    {
        IanMusicMag mag = (IanMusicMag)AnopiaAudioCore.FetchMag(songID);
        NewSong(mag);
        CurrentSong.FadeIn(t);
        AnopiaSynchro.StartSynchro(this, mag.Tempo, CurrentSong.Play);
    }
    public void TransitionSnapshot(string snapshot,float time)//static func?
    {
        MusicMixer.TransitionToSnapshot(snapshot, time);
    }
    public void Mute(bool toMute)
    {
        CurrentSong.Mute(toMute);
    }
}