using System;
using UnityEngine;
public class YgniumSynchro : IanSynchro
{
    public AudioSource Source;
    Action _Ygnite, _Shadow;
    public int CurrentBarCount;
    public override void StartSynchro(double startTime)
    {
        StopSynchro();
        SparkYgnium(1, 1, startTime);
    }
    public override void StopSynchro()
    {
        Source.Stop();
        _Ygnite = null;
        CurrentBeatCount = CurrentBarCount = 0;
    }
    void SparkYgnium(int beat, int bar, double timeCode)
    {
        Source.PlayScheduled(timeCode);
        print(timeCode-AudioSettings.dspTime);//must be a positive value
        _Ygnite = () => Ygnite(beat, bar, timeCode);
        _Shadow = Shadow;
    }
    void Ygnite(int beat, int bar, double timeCode)
    {
        PlayOnBeat?.Invoke(beat, timeCode);
        CurrentBeatCount = beat;
        CurrentBarCount = bar;
        CurrentBeat = timeCode;
        _Shadow = Shadow;
    }
    void Shadow()
    {
        SparkYgnium(CurrentBeatCount + 1, CurrentBarCount + 1, CurrentBeat + Tempo.BeatLength);
    }
    void Update()
    {
        if (Source.time > 0)
        {
            _Ygnite?.Invoke();
            _Ygnite = null;
        }
        else if(!Source.isPlaying)
        {
            _Shadow?.Invoke();
            _Shadow = null;
        }
        CurrentDSPTime = AudioSettings.dspTime;
        SourceTime = Source.time;
        SourcePlaying = Source.isPlaying;
    }
    public double CurrentDSPTime, SourceTime;
    public bool SourcePlaying;
}