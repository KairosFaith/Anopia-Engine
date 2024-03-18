using System;
using UnityEngine;
using UnityEngine.Audio;
public class anLinearSong : IanSong
{
    anSourcerer CurrentMainSource;
    AudioMixerGroup Output;
    anLinearMusicMag Mag;
    public override void Setup(IanMusicMag mag, AudioMixerGroup output)
    {
        Output = output;
        Mag = (anLinearMusicMag)mag;
    }
    void NewCurrentSource(AudioClip clip)
    {
        CurrentMainSource = anCore.Setup2DLoopSource(clip, Output);
        CurrentMainSource.audioSource.outputAudioMixerGroup = Output;
    }
    void StopCurrentSource(double stopTime)
    {
        CurrentMainSource.audioSource.SetScheduledEndTime(stopTime);
        CurrentMainSource.DeleteAfterTime(stopTime);
    }
    public override void Play(double startTime)
    {
        AudioClip introClip = Mag.Intro;
        double introLength = 0;
        if(introClip != null)
        {
            introLength = introClip.length;
            anCore.PlayClipScheduled(introClip, startTime, Output);
        }
        NewCurrentSource(Mag.MainSection);
        CurrentMainSource.audioSource.PlayScheduled(startTime + introLength);
    }
    public void CueSection(int key)
    {
        SongSection toPlay = Mag.Sections[key];
        CueSection(toPlay);
    }
    void CueSection(SongSection toPlay)
    {
        anSynchro instance = anSynchro.Instance;
        void NextSong(double timeCode)
        {
            CurrentMainSource.audioSource.SetScheduledEndTime(timeCode);
            CurrentMainSource.DeleteAfterTime(timeCode);
            NewCurrentSource(toPlay.Section);
            CurrentMainSource.audioSource.PlayScheduled(timeCode);
        };
        if (toPlay.Stinger != null)
        {
            if (toPlay.BeatToStart != 0)
            {
                void PlayStinger(int beatcount, double timeCode)
                {
                    if (beatcount == toPlay.BeatToStart)
                    {
                        anCore.PlayClipScheduled(toPlay.Stinger, timeCode, Output);
                        NextSong(instance.CurrentBar + instance.Tempo.BarLength);
                        anSynchro.PlayOnBeat -= PlayStinger;
                    }
                };
                anSynchro.PlayOnBeat += PlayStinger;
                return;
            }
            else
                anCore.PlayClipScheduled(toPlay.Stinger, instance.CurrentBar + instance.Tempo.BarLength, Output);
        }
            NextSong(instance.CurrentBar + instance.Tempo.BarLength);
    }
    public void CueFinal(Action onDone = null)
    {
        anSynchro instance = anSynchro.Instance;
        SongSection toPlay = Mag.Final;
        void NextSong(double timeCode)
        {
            StopCurrentSource(timeCode);
            onDone += () => Destroy(gameObject);
            anSourcerer a = anCore.PlayClipScheduled(toPlay.Section, timeCode, Output);
            a.DeleteWhenDone(onDone);
        };
        if (toPlay.Stinger != null)
        {
            void PlayStinger(int beatcount, double timeCode)
            {
                if (beatcount == toPlay.BeatToStart)
                {
                    anCore.PlayClipScheduled(toPlay.Stinger, timeCode, Output);
                    NextSong(instance.CurrentBar + instance.Tempo.BarLength);
                    anSynchro.PlayOnBeat -= PlayStinger;
                }
            };
            anSynchro.PlayOnBeat += PlayStinger;
        }
        else
            NextSong(instance.CurrentBar + instance.Tempo.BarLength);
    }
    public override void StopOnCue(double stopTime)
    {
        StopCurrentSource(stopTime);
        transform.DetachChildren();
        Destroy(gameObject);
    }
    public override void StopImmediate()
    {
        CurrentMainSource.audioSource.Stop();
        Destroy(gameObject); 
    }
    public override void FadeOut(float t, Action ondone = null)
    {
        AudioSource s = CurrentMainSource.audioSource;
        void Fade(float v)
        {
            s.volume = v;
        };
        ondone += () => Destroy(gameObject);
        StartCoroutine(anCore.FadeValue(t, 1, 0, Fade, ondone));
    }
    public override void Mute(bool toMute)
    {
        CurrentMainSource.audioSource.mute = toMute;
    }
    public override void FadeIn(float t)
    {
        throw new NotImplementedException("Fade in not available for Linear Song Intro");
    }
}