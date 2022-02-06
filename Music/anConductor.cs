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
    //for debug mode or put public
    public int BeatCount;
    double LastBeat,NextBeat, NextBar;
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
    public void CueSong(string songID, double timeCode)//user defined cue, assume same tempo
    {
        if (_CurrentSong != null)
            _CurrentSong.StopOnCue(timeCode);
        IanMusicMag mag = (IanMusicMag)anCore.FetchMag(songID);
        NewSong(mag);
        _CurrentSong.Play(timeCode);
    }
    public void CueSection(int key)
    {
        if (_CurrentSong is anLinearSong ls)
            ls.CueSection(key);
    }
    public void CueFinal()
    {
        if (_CurrentSong is anLinearSong ls)
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
                ls.Setup(mag, MusicMixer.outputAudioMixerGroup);
                _CurrentSong = ls;
                break;
            case SongForm.Stem:
                anStemSong stem = g.AddComponent<anStemSong>();
                stem.Setup(mag);
                _CurrentSong = stem;
                break;
        }
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
    public void FadeOut(float t, Action onDone)
    {
        _CurrentSong.FadeOut(t, onDone); 
        anSynchro.StopSynchro();
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