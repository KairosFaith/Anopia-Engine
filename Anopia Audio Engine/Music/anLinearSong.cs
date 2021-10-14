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
        NewSource();
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
        CueSection(toPlay);
    }
    void CueSection(SongSection toPlay)
    {
        void NextSong(double timeCode)
        {
            CurrentMainSource.StopScheduled(timeCode, true);
            NewSource();
            CurrentMainSource.PlayScheduled(toPlay.Loop, timeCode);
        };
        if (toPlay.Stinger != null)
        {
            void PlayStinger(int beatcount, double timeCode)
            {
                if (beatcount == toPlay.BeatTostart)
                {
                    AnopiaAudioCore.PlayClipScheduled(this, toPlay.Stinger, timeCode, Output);
                    NextSong(AnopiaSynchro.NextBar);
                    AnopiaSynchro.PlayOnBeat -= PlayStinger;
                }
            };
            AnopiaSynchro.PlayOnBeat += PlayStinger;
        }
        else
            NextSong(AnopiaSynchro.NextBar);
    }
    public void CueFinal()
    {
        SongSection toPlay = Mag.Final;
        void NextSong(double timeCode)
        {
            CurrentMainSource.StopScheduled(timeCode, true);
            CurrentMainSource = AnopiaAudioCore.NewStereoSource(this, Output);
            CurrentMainSource.PlayScheduled(toPlay.Loop, timeCode); 
            AudioSource a = CurrentMainSource.Source;
            a.loop = false;
            transform.DetachChildren();
            Destroy(gameObject);
            void onDone()
            {
                AnopiaSynchro.StopSynchro(AnopiaConductor.Instance);
            };
            CurrentMainSource.StartCoroutine(AnopiaAudioCore.DeleteWhenDone(a, onDone));
        };
        if (toPlay.Stinger != null)
        {
            void PlayStinger(int beatcount, double timeCode)
            {
                if (beatcount == toPlay.BeatTostart)
                {
                    AnopiaAudioCore.PlayClipScheduled(this, toPlay.Stinger, timeCode, Output);
                    NextSong(AnopiaSynchro.NextBar);
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
        ondone += () => Destroy(gameObject);
        StartCoroutine(AnopiaAudioCore.FadeValue(t, 1, 0, Fade, ondone));
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
    public override void Mute(bool toMute)
    {
        CurrentMainSource.Source.mute = toMute;
    }
}