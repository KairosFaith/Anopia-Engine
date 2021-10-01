using System;
using System.Collections;
using UnityEngine;
public class HorizontalMusic : SingletonMonobehavior<HorizontalMusic>
{
    public AudioSource MainSource, SideSource;
    public SongSet[] Songs;
    public Transition[] Stingers;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
    void Start()
    {
        MainSource.loop = true;
        SideSource.loop = false;
    }
    public void PlaySongIntro(int Key)
    {
        if(MainSource.isPlaying)
           MainSource.Stop();
        SongSet toPlay = Songs[Key];
        TempoData tempo = toPlay.Tempo;
        MainSource.clip = toPlay.Main;
        AudioClip introClip = toPlay.Intro;
        SideSource.clip = introClip;
        void Onstart(double introTime)
        {
            SideSource.PlayScheduled(introTime);
            //MainSource.PlayScheduled(introTime + introClip.length);
            MainSource.PlayScheduled(introTime + tempo.BeatLength * toPlay.IntroBeats);
        }
        AnopiaConductor.BeginConductor(this, tempo, Onstart);
    }
    public void PlaySongStinger(int Key)//stingers do not change tempodata
    {
        Transition toPlay = Stingers[Key];
        void PlayStinger(double fillTime)
        {
            int beatToCheck = toPlay.BeatTostart - 1;
            if (beatToCheck == 0)
                beatToCheck = AnopiaConductor.Tempo.BeatsPerBar;
            if (AnopiaConductor.BeatCount==toPlay.BeatTostart-1)
            {
                AudioClip fill = toPlay.Stinger;
                SideSource.clip = fill;
                SideSource.PlayScheduled(fillTime);
                MainSource.clip = toPlay.Main;
                MainSource.PlayScheduled(fillTime + AnopiaConductor.Tempo.BeatLength * toPlay.StingerBeats);
                //MainSource.PlayScheduled(fillTime + fill.length);
                AnopiaConductor.PlayOnBeat -= PlayStinger;
            }
        };
        AnopiaConductor.PlayOnBeat += PlayStinger;
    }
    public void PlayInterrupt(AudioClip clip)
    {
        StartCoroutine(Interrupt(clip));
    }
    IEnumerator Interrupt(AudioClip clip)
    {
        MainSource.Pause();
        SideSource.PlayOneShot(clip);
        yield return new WaitForSecondsRealtime(clip.length);
        while(SideSource.isPlaying)
            yield return new WaitForEndOfFrame();
        MainSource.UnPause();
    }
}
[Serializable]
public class SongSet
{
    public AudioClip Intro, Main;
    public int IntroBeats;
    public TempoData Tempo;
}
[Serializable]
public class Transition
{
    public AudioClip Stinger, Main;
    public int BeatTostart,StingerBeats;
}