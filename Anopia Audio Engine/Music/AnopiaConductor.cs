using UnityEngine;
using UnityEngine.Audio;
public class AnopiaConductor : MonoBehaviour
{
    IanSong CurrentSong;
    //static set through datacore
    public static AudioMixerGroup MainChannel;
    public static AudioMixer MusicMixer;//for snapshots
    public static AudioMixerGroup[] StemChannels;//for using stems
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
    public void ChangeSection(int key)
    {//stinger is not played
        if (CurrentSong is anLinearSong ls)
            ls.ChangeSectionImmediate(key);
    }
    public void CueSection(int key)
    {
        if (CurrentSong is anLinearSong ls)
            ls.CueSection(key);
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
    #region Pause/Interrupt Functions
    //WARNING Pausing and unpausing will mess up the Synchro!!!!
    //Or any playscheduled sounds
    //Use At Own Risk
    public void Pause()
    {
        CurrentSong.Pause();
    }
    public void UnPause()
    {
        CurrentSong.UnPause();
    }
    public void Interrupt(string IDtag,int key,float panStereo = 0)
    {
        Pause();
        anClipMag mag = (anClipMag)AnopiaAudioCore.FetchMag(IDtag);//Use Clip Mag
        ClipData[] array = mag.Data;
        ClipData c = array[key];
        AnopiaAudioCore.PlayClipAtStereo(this, c, MainChannel, panStereo, ()=>UnPause());
    }
    #endregion
}