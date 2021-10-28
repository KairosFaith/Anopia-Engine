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
        AnopiaSynchro.StopSynchro(this);
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