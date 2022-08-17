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
            anSynchro.Instance.StopAllCoroutines();
        }
        IanMusicMag mag = (IanMusicMag)anCore.FetchMag(songID);
        NewSong(mag);
        anSynchro.StartSynchro(AudioSettings.dspTime);
        _CurrentSong.Play(AudioSettings.dspTime);
        //return _CurrentSong;
    }
    public void CueSong(string songID)//cue song on the next bar, resync to new tempo
    {
        anSynchro instance = anSynchro.Instance;
        if (_CurrentSong != null)
            _CurrentSong.StopOnCue(instance.CurrentBar + instance.Tempo.BarLength);
        IanMusicMag mag = (IanMusicMag)anCore.FetchMag(songID);
        NewSong(mag);
        anSynchro.PlayOnBeat += ( int beatcount, double timecode) =>
        {
            if (beatcount == 1)
            {
                instance.Tempo = mag.Tempo;
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
            onDone += () => anSynchro.Instance.StopAllCoroutines();
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
        anSynchro.Instance.StopAllCoroutines();
    }
    public void Crossfade(float t, string songID)
    {
        _CurrentSong.FadeOut(t, ()=> FadeIn(t, songID));
    }
    public void FadeOut(float t, Action onDone = null)
    {
        anSynchro.Instance.StopAllCoroutines();
        _CurrentSong.FadeOut(t, onDone);
    }
    public void FadeIn(float t, string songID)
    {
        IanMusicMag mag = (IanMusicMag)anCore.FetchMag(songID);
        NewSong(mag);
        _CurrentSong.FadeIn(t);
        anSynchro.StartSynchro(AudioSettings.dspTime+ Time.deltaTime);
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