using System;
using System.Collections;
using UnityEngine;
public delegate void BeatFunc(int beatCount, double timeCode);
public class anSynchro: MonoBehaviour //this is your new update engine
{
    public static anSynchro Instance { get; private set; }
    public static BeatFunc PlayOnBeat;// to schedule sounds
    public anTempoData Tempo;
    [Header("Read Only")]
	public int CurrentBeatCount = 0;
	public double NextBeat, CurrentBar;//current bar is past
	public bool SynchroActive;
    static Coroutine _SynchroRoutine;
	public static double BeatLength => Instance.Tempo.BeatLength;
	public static double BarLength => Instance.Tempo.BarLength;
	public static double NextBar => Instance.CurrentBar + BarLength;
    private void Awake()
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
    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    public static void StartSynchro(double startTime)
    {
        StopSynchro();
        _SynchroRoutine = Instance.StartCoroutine(Instance.Synchro(startTime));
    }
    public static void StopSynchro()
    {
        Instance.StopCoroutine(_SynchroRoutine);
        Instance.SynchroActive = false;
    }
    IEnumerator Synchro(double startTime)
    {
        SynchroActive = true;
        CurrentBeatCount = 0;
        CurrentBar = NextBeat = startTime;
        void Beat(double beatTimeCode)
        {
            if (CurrentBeatCount == Tempo.BeatsPerBar)
            {
                CurrentBeatCount = 1;
                CurrentBar = beatTimeCode;
            }
            else
                CurrentBeatCount++;
            NextBeat += Tempo.BeatLength;
            PlayOnBeat?.Invoke(CurrentBeatCount, beatTimeCode);
        };
        while (SynchroActive)
        {
            //check 1 frame ahead, actual dsp time must be LOWER than NextBeat Time code
            if (AudioSettings.dspTime +Time.deltaTime >= NextBeat)
                Beat(NextBeat);
            yield return new WaitForEndOfFrame();
        }
        SynchroActive = false;
    }
    public bool CheckRhythmAcurracy(float marginOfError = 0.4f)
    {
        double pressedTimecode = AudioSettings.dspTime;
        if(!SynchroActive)
            return false;
        if(CheckRhythmDelta(pressedTimecode, NextBeat, marginOfError))
            return true;
        return CheckRhythmDelta(pressedTimecode, NextBeat - Tempo.BeatLength, marginOfError);
    }
    bool CheckRhythmDelta(double pressedTimeCode, double timeCodeToCheck, float marginOfError)
    {
        double diff = pressedTimeCode - timeCodeToCheck;
        float delta = (float)(diff / Tempo.BeatLength);
        return MathF.Abs(delta) < marginOfError;
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
}