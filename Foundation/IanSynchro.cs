using System;
using UnityEngine;
public delegate void BeatFunc(int beatCount, double timeCode);
public abstract class IanSynchro : MonoBehaviour
{
    public static IanSynchro Instance { get; protected set; }
    public static BeatFunc PlayOnBeat;// to schedule sounds
    public anTempoData Tempo;
    public static double BeatLength => Instance.Tempo.BeatLength;
    public static double BarLength => Instance.Tempo.BarLength;
    public static double NextBar => Instance.CurrentBar + BarLength;
    [Header("Read Only")]
    public int CurrentBeatCount, CurrentBarCount;
    public double NextBeat, CurrentBar;//current bar is past
    public abstract void StartSynchro(double startTime);
    public abstract void StopSynchro();
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
        {
            Debug.LogWarning(Instance.gameObject.name + "Synchro Already exists");
            Destroy(this);
        }
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
    public float BeatLength => 60 / (float)BPM;
    public float BarLength => BeatLength * BeatsPerBar;
}