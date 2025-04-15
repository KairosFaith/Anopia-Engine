using System;
using UnityEngine;
public delegate void BeatFunc(int beatCount, double timeCode);
public abstract class IanSynchro : MonoBehaviour
{
    public static IanSynchro Instance { get; protected set; }
    public BeatFunc PlayOnBeat;// to schedule sounds
    public anTempoData Tempo;
    [Header("Read Only")]
    public int CurrentBeatCount;
    public double NextBeat, CurrentBar;//current bar is past
    public abstract void StartSynchro(double startTime);
    public abstract void StopSynchro();
    public double GetNextBar(int barsAhead = 1)
    {
        return CurrentBar + Tempo.BarLength * barsAhead;
    }
    public bool CheckRhythmAcurracy(float marginOfError = 0.4f)
    {
        double pressedTimecode = AudioSettings.dspTime;
        if (CheckRhythmDelta(pressedTimecode, NextBeat, marginOfError))
            return true;
        return CheckRhythmDelta(pressedTimecode, NextBeat - Tempo.BeatLength, marginOfError);
    }
    bool CheckRhythmDelta(double pressedTimeCode, double timeCodeToCheck, float marginOfError)
    {
        double diff = pressedTimeCode - timeCodeToCheck;
        float delta = (float)(diff / Tempo.BeatLength);
        return Mathf.Abs(delta) < marginOfError;
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
[Serializable]
public class anTempoData
{
    public enum anBarValue
    {
        Quarter = 1,
        Eight,
        Triplet,
        Sixteen,
    }
    public int CrotchetBPM = 65;
    public int BeatsPerBar = 4;
    public anBarValue TimeSignature = anBarValue.Quarter;
    public int BPM => CrotchetBPM * (int)TimeSignature;
    public double BeatLength => (double)60 / BPM;
    public double BarLength => BeatLength * BeatsPerBar;
}