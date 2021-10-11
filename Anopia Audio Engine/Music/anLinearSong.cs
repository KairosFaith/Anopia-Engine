using System;
using UnityEngine;
using UnityEngine.Audio;
public class anLinearSong : IanSong
{
    AnopiaSourcerer CurrentMainSource;
    AudioMixerGroup Output;
    anLinearMusicMag Mag;
    public void Setup(IanMusicMag mag, AudioMixerGroup output)
    {
        Output = output;
        Mag = (anLinearMusicMag)mag;
        NewSource();
    }
    void NewSource()
    {
        CurrentMainSource = AnopiaAudioCore.NewStereoSource(this, Output);
        CurrentMainSource.Source.loop = true;
    }
    public override void Play(double startTime)
    {
        AudioClip introClip = Mag.Intro;
        double introLength = 0;
        if(introClip != null)
        {
            introLength = introClip.length;
            AnopiaAudioCore.PlayClipScheduled(this, introClip, startTime, Output);
        }
        CurrentMainSource.PlayScheduled(Mag.MainSection, startTime + introLength);
    }
    public void ChangeSectionImmediate(int key)
    {
        AudioSource s = CurrentMainSource.Source;
        s.Stop();
        SongSection toPlay = Mag.Sections[key];
        s.clip = toPlay.Loop;
        s.Play();
    }
    public void CueSection(int key)
    {
        SongSection toPlay = Mag.Sections[key];
        void NextSong(double timeCode)
        {
            CurrentMainSource.StopScheduled(timeCode, true);
            NewSource();
            CurrentMainSource.PlayScheduled(toPlay.Loop, timeCode);
        };
        if (toPlay.Stinger != null)
        {
            void PlayStinger(double timeCode)
            {
                double startSongTime = AnopiaSynchro.NextBar;
                int beatToCheck = toPlay.BeatTostart - 1;
                if (beatToCheck == 0)
                {
                    beatToCheck = AnopiaSynchro.Tempo.BeatsPerBar;
                    startSongTime += AnopiaSynchro.Tempo.BarLength;
                }
                if (AnopiaSynchro.BeatCount == beatToCheck)
                {
                    AnopiaAudioCore.PlayClipScheduled(this, toPlay.Stinger, timeCode, Output);
                    NextSong(startSongTime);
                    AnopiaSynchro.PlayOnBeat -= PlayStinger;
                }
            };
            AnopiaSynchro.PlayOnBeat += PlayStinger;
        }
        else
            NextSong(AnopiaSynchro.NextBar);
    }
    public override void StopCue(double stopTime)
    {
        CurrentMainSource.StopScheduled(stopTime,true);//this will destroy the source after it has finished playing
        transform.DetachChildren();
        Destroy(gameObject);
    }
    public override void StopImmediate()
    {
        CurrentMainSource.Source.Stop();
        Destroy(gameObject);
    }
    public override void FadeOut(float t, Action ondone = null)
    {
        AudioSource s = CurrentMainSource.Source;
        void Fade(float v)
        {
            s.volume = v;
        };
        StartCoroutine(AnopiaAudioCore.FadeValue(t, 1, 0, Fade, ondone += () => Destroy(gameObject) ));
    }
    public override void Pause()
    {
        CurrentMainSource.Source.Pause();
    }
    public override void UnPause()
    {
        CurrentMainSource.Source.UnPause();
    }
    public override void FadeIn(float t)
    {
        throw new NotImplementedException("Fade in not available for Linear Intro Song");
    }
}