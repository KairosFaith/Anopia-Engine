using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AnopiaConductor : SingletonMonobehavior<AnopiaConductor>
{
    IanSong CurrentSong;
    public int CurrentBeat;
    //static set through datacore
    public AudioMixerGroup MainChannel;
    public AudioMixer MusicMixer;//for snapshots
    public AudioMixerGroup[] StemChannels;//for using stems
    protected override void Awake()
    {
        base.Awake();
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
            AnopiaSynchro.StopSynchro(this);
        }
        IanMusicMag mag = (IanMusicMag)AnopiaAudioCore.FetchMag(songID);
        NewSong(mag);
        AnopiaSynchro.StartSynchro(this, mag.Tempo, CurrentSong.Play);
        return CurrentSong;
    }
    public void CueSong(string songID, double timeCode)//user defined cue
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
        AnopiaSynchro.StopSynchro(this);
    }
    public void FadeOut(float t)
    {
        CurrentSong.FadeOut(t);
    }
    public void FadeChange(string newSongID,float t)
    {
        void afterFadeOut()
        {
            CurrentSong.FadeIn(t);
            CurrentSong.Play(AudioSettings.dspTime+Time.deltaTime);
        };
        CurrentSong.FadeOut(t, afterFadeOut);
        IanMusicMag mag = (IanMusicMag)AnopiaAudioCore.FetchMag(newSongID);
        NewSong(mag);
    }
    public void TransitionSnapshot(string snapshot,float time)//static func?
    {
        MusicMixer.TransitionToSnapshot(snapshot, time);
    }
    public void Mute(bool toMute)
    {
        CurrentSong.Mute(toMute);
    }
    public void Interrupt(string IDtag, int key, float panStereo = 0)
    {
        Mute(true);
        anClipMag mag = (anClipMag)AnopiaAudioCore.FetchMag(IDtag);//Use Clip Mag
        ClipData[] array = mag.Data;
        ClipData c = array[key];
        AnopiaAudioCore.PlayClipAtStereo(this, c, MainChannel, panStereo, () => Mute(false));
    }
    #region Pause Interrupt Functions (Use At Own Risk)
    //WARNING Pausing and unpausing does not stop DSP Time!!!!
    //synchro will no longer align
    public void ChangeSection(int key)
    {//immediate stinger is not played
        if (CurrentSong is anLinearSong ls)
            ls.ChangeSectionImmediate(key);
        //TODO restart synchro?
    }
    public void Pause(bool toPause)
    {
        if(toPause)
           CurrentSong.Pause();
        else
            CurrentSong.UnPause();
    }
    public void InterruptPause(string IDtag,int key,float panStereo = 0)
    {
        Pause(true);
        anClipMag mag = (anClipMag)AnopiaAudioCore.FetchMag(IDtag);//Use Clip Mag
        ClipData[] array = mag.Data;
        ClipData c = array[key];
        AnopiaAudioCore.PlayClipAtStereo(this, c, MainChannel, panStereo, ()=>Pause(false));
    }
    #endregion
}