using System;
using System.Collections;
using UnityEngine;
public delegate void ClickFunction(double time);
public delegate void TriggerDelegate();
public static class AnopiaConductor//this is your new update engine
{
    public static TempoData Tempo;
    static bool isPlaying;
    static double StartTime;
    public static int BeatCount => _BeatCount;
    static int _BeatCount = 0;
    public static double NextBeat,NextBar;
    public static ClickFunction PlayOnBeat, PlayOnBar;// to schedule sounds
    public static TriggerDelegate OnBeat, OnBar, Unschedule;//for anything else
    public static void BeginConductor(MonoBehaviour host, TempoData tempo, Action<double>OnStart = null)
    {
        isPlaying = true;
        Tempo = tempo;
        StartTime = AudioSettings.dspTime + Time.deltaTime;//start next frame
        host.StartCoroutine(Conductor(StartTime));
        OnStart?.Invoke(StartTime);
    }
    static IEnumerator Conductor(double startTime)
    {
        PlayOnBeat?.Invoke(startTime);
        PlayOnBar?.Invoke(startTime);
        OnBeat?.Invoke();
        OnBar?.Invoke();
        NextBeat = startTime + Tempo.BeatLength;
        NextBar = startTime + Tempo.BarLength;
        _BeatCount = 1;
        PlayOnBeat?.Invoke(NextBeat);
        PlayOnBar?.Invoke(NextBar);
        void Beat()
        {
            NextBeat += Tempo.BeatLength;
            _BeatCount++;
            PlayOnBeat?.Invoke(NextBeat);
            if (_BeatCount > Tempo.BeatsPerBar)
                Bar();
            OnBeat?.Invoke();
        };
        void Bar()
        {
            NextBar += Tempo.BarLength;
            _BeatCount = 1;
            PlayOnBar?.Invoke(NextBar);
            OnBar?.Invoke();
        };
        while (isPlaying)
        {
            yield return new WaitForEndOfFrame();
            if (AudioSettings.dspTime > NextBeat)
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
    Sixteen,
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