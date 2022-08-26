using System;
using System.Collections;
using UnityEngine;
public delegate void BeatFunc(int beatCount, double timeCode);
public class anSynchro: MonoBehaviour //this is your new update engine
{
    public static anSynchro Instance;
    public static BeatFunc PlayOnBeat;// to schedule sounds
    public anTempoData Tempo;
    public int CurrentBeatCount = 0;
    public double NextBeat;
    public double CurrentBar;//start time of current bar, past
    public bool SynchroActive;
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
        Instance.StartCoroutine(Instance.Synchro(startTime));
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
}
public enum anBarValue
{
    Quarter = 1,
    Eight,
    Sixteen = 4,
}
[Serializable]
public class anTempoData
{
    public int CrotchetBPM = 65;
    public int BeatsPerBar = 4;
    public anBarValue TimeSignature = anBarValue.Quarter;
    public int BPM => CrotchetBPM * (int)TimeSignature;
    public float BeatLength => 60 / (float)BPM;
    public float BarLength => BeatLength * BeatsPerBar;
}