using System;
using UnityEngine;
using UnityEngine.Audio;
public class anConductor : MonoBehaviour
{
    static anConductor _Instance;
    public static anConductor Instance => _Instance;
    IanSong _CurrentSong;
    //call this for snapshots and routing
    public AudioMixer MusicMixer;
#if UNITY_EDITOR
    //for inspector view only, set public or use debug mode
    int BeatCount;
    double LastBeat,NextBeat, NextBar;
    private void Start()
    {
        anSynchro.PlayOnBeat += (int beatcount, double timecode) => 
        { 
            BeatCount = beatcount;
            LastBeat = timecode;
            NextBeat = anSynchro.NextBeat;
            NextBar = anSynchro.NextBar;
        };
    }
#endif
    protected void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    protected void OnDestroy()
    {
        if (_Instance == this)
            _Instance = null;
    }
    public void PlayNewSong(string songID)//play immediate
    {
        if (_CurrentSong != null)
        {
            _CurrentSong.StopImmediate();
            anSynchro.StopSynchro();
        }
        IanMusicMag mag = (IanMusicMag)anCore.FetchMag(songID);
        NewSong(mag);
        anSynchro.StartSynchro(this, mag.Tempo, _CurrentSong.Play);
        //return _CurrentSong;
    }
    public void CueSong(string songID)//cue song on the next bar, resync to new tempo
    {
        if (_CurrentSong != null)
            _CurrentSong.StopOnCue(anSynchro.NextBar);
        IanMusicMag mag = (IanMusicMag)anCore.FetchMag(songID);
        NewSong(mag);
        anSynchro.PlayOnBeat += ( int beatcount, double timecode) =>
        {
            if (beatcount == 1)
            {
                anSynchro.ReSync(mag.Tempo);
                CueSong(songID, timecode);
            }
        };
    }
    public void CueSong(string songID, double timeCode)//user defined cue, assume same tempo... Or not
    {
        if (_CurrentSong != null)
            _CurrentSong.StopOnCue(timeCode);
        IanMusicMag mag = (IanMusicMag)anCore.FetchMag(songID);
        NewSong(mag);
        //anSynchro.ReSync(mag.Tempo);
        _CurrentSong.Play(timeCode);
    }
    public void CueSection(int key)
    {
        if (_CurrentSong is anLinearSong ls)
            ls.CueSection(key);
    }
    public void CueFinal(Action onDone = null)
    {
        if (_CurrentSong is anLinearSong ls)
        {
            onDone += () => anSynchro.StopSynchro();
            ls.CueFinal(onDone);
        }
    }
    void NewSong(IanMusicMag mag)
    {
        GameObject g = new GameObject(mag.name);
        g.transform.position = transform.position;
        g.transform.SetParent(transform);
        switch (mag.Structure)
        {
            case SongForm.Linear:
                _CurrentSong = g.AddComponent<anLinearSong>();
                break;
            case SongForm.Stem:
                _CurrentSong = g.AddComponent<anStemSong>();
                break;
        }
        _CurrentSong.Setup(mag, MusicMixer.outputAudioMixerGroup);
    }
    public void Stop(double stopTime)//user defined cue
    {
        _CurrentSong.StopOnCue(stopTime);
        anSynchro.StopSynchro();
    }
    public void Crossfade(float t, string songID)
    {
        _CurrentSong.FadeOut(t, ()=> FadeIn(t, songID));
    }
    public void FadeOut(float t, Action onDone = null)
    {
        anSynchro.StopSynchro();
        _CurrentSong.FadeOut(t, onDone);
    }
    public void FadeIn(float t, string songID)
    {
        IanMusicMag mag = (IanMusicMag)anCore.FetchMag(songID);
        NewSong(mag);
        _CurrentSong.FadeIn(t);
        anSynchro.StartSynchro(this, mag.Tempo, _CurrentSong.Play);
    }
    public void TransitionSnapshot(string snapshot,float time)//TODO static func?
    {
        MusicMixer.TransitionToSnapshot(snapshot, time);
    }
    public void Mute(bool toMute)
    {
        _CurrentSong.Mute(toMute);
    }
}