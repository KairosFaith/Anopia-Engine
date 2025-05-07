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
    public double CurrentBeat, CurrentBar, NextBeat;//current bar is past
    public abstract void StartSynchro(double startTime);
    public abstract void StopSynchro();
    public double GetNextBar(int barsAhead = 1)
    {
        return CurrentBar + Tempo.BarLength * barsAhead;
    }
    ///<summary>Based on proportional margin</summary>
    public bool CheckRhythmAcurracy(out bool isEarly, float marginOfError = 0.4f)
    {
        double pressedTimecode = AudioSettings.dspTime;
        double inverseLerp = (pressedTimecode - CurrentBeat) / (NextBeat - CurrentBeat);
        if (inverseLerp <= marginOfError)
        {
            isEarly = false;
            return true;
        }
        isEarly = inverseLerp >= (1 - marginOfError);
        return isEarly;
    }
    ///<summary>Based on fixed time gap</summary>
    public bool CheckRhythmAcurracy(out bool isEarly, double marginOfError)
    {
        double pressedTimecode = AudioSettings.dspTime;
        if ((pressedTimecode - CurrentBeat) <= marginOfError)
        {
            isEarly = false;
            return true;
        }
        isEarly = (NextBeat - pressedTimecode) <= marginOfError;
        return isEarly;
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
    public int CrotchetBPM = 120;
    public int BeatsPerBar = 4;
    public anBarValue TimeSignature = anBarValue.Quarter;
    public int BPM => CrotchetBPM * (int)TimeSignature;
    public double BeatLength => (double)60 / BPM;
    public double BarLength => BeatLength * BeatsPerBar;
}