using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
public delegate void ClickFunction(double time);
public delegate void TriggerDelegate();
public static class AnopiaSynchro//this is your new update engine
{
    public static TempoData Tempo;
    static bool isPlaying;
    static double StartTime;
    public static int BeatCount => _BeatCount;
    public static double NextBeat => _NextBeat;
    public static double NextBar => _NextBar;
    static int _BeatCount = 0;
    static double _NextBeat,_NextBar;
    public static ClickFunction PlayOnBeat, PlayOnBar;// to schedule sounds
    public static TriggerDelegate OnBeat, OnBar, Unschedule;//for anything else
    public static void StartSynchro(MonoBehaviour host, TempoData tempo, Action<double>OnStart = null)
    {
        isPlaying = true;
        Tempo = tempo;
        StartTime = AudioSettings.dspTime + Time.deltaTime;//start next frame
        host.StartCoroutine(Synchro(StartTime));
        OnStart?.Invoke(StartTime);
    }
    public static void StopSynchro(MonoBehaviour host)
    {
        host.StopAllCoroutines();
    }
    static IEnumerator Synchro(double startTime)
    {
        PlayOnBeat?.Invoke(startTime);
        PlayOnBar?.Invoke(startTime);
        OnBeat?.Invoke();
        OnBar?.Invoke();
        _NextBeat = startTime + Tempo.BeatLength;
        _NextBar = startTime + Tempo.BarLength;
        _BeatCount = 1;
        PlayOnBeat?.Invoke(_NextBeat);
        PlayOnBar?.Invoke(_NextBar);
        void Beat()
        {
            _NextBeat += Tempo.BeatLength;
            _BeatCount++;
            PlayOnBeat?.Invoke(_NextBeat);
            if (_BeatCount > Tempo.BeatsPerBar)
                Bar();
            OnBeat?.Invoke();
        };
        void Bar()
        {
            _NextBar += Tempo.BarLength;
            _BeatCount = 1;
            PlayOnBar?.Invoke(_NextBar);
            OnBar?.Invoke();
        };
        while (isPlaying)
        {
            yield return new WaitForEndOfFrame();
            if (AudioSettings.dspTime > _NextBeat)
                Beat();
        }
        Unschedule?.Invoke();
        _BeatCount = 0;
    }
}
public enum BarValue
{
    Quarter = 1,
    Eight,
    Sixteen = 4,
}
[Serializable]
public class TempoData
{
    public int CrotchetBPM;
    public int BeatsPerBar;
    public BarValue TimeSignature;
    public int BPM => CrotchetBPM * (int)TimeSignature;
    public float BeatLength => 60 / (float)BPM;
    public float BarLength => BeatLength * BeatsPerBar;
}
public abstract class IanMusicMag : IanAudioMag
{
    public TempoData Tempo;
    public abstract SongForm Structure { get; }
    public override IanEvent LoadMag(MonoBehaviour host, AudioMixerGroup output)
    {
        throw new System.NotImplementedException();
    }
}
public enum SongForm
{
    Linear,
    Stem
}
public abstract class IanSong : MonoBehaviour
{
    public abstract void StopCue(double stopTime);
    public abstract void StopImmediate();
    public abstract void Play(double startTime);
    public abstract void FadeIn(float t);
    public abstract void FadeOut(float t, Action ondone = null);
    //WARNING Pausing and unpausing will mess up the Synchro!!!!
    public abstract void Pause();
    public abstract void UnPause();
}