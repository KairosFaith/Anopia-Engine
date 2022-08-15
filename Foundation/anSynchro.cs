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
    static Coroutine CoroutineInstance;
    public bool IsPlaying;
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
        CoroutineInstance = Instance.StartCoroutine(Instance.Synchro(startTime));
    }
    public static void StopSynchro()
    {
        Instance.StopCoroutine(CoroutineInstance);
    }
    IEnumerator Synchro(double startTime)
    {
        IsPlaying = true;
        CurrentBeatCount = 0;
        CurrentBar = NextBeat = startTime;
        void Beat(double beatTimeCode)
        {
            bool reachedBarLine = CurrentBeatCount == Tempo.BeatsPerBar;
            if (reachedBarLine)
            {
                CurrentBeatCount = 1;
                CurrentBar = beatTimeCode;
            }
            else
                CurrentBeatCount++;
            PlayOnBeat?.Invoke(CurrentBeatCount, beatTimeCode);
            NextBeat += Tempo.BeatLength;
        };
        while (IsPlaying)
        {
            //check 1 frame ahead, actual dsp time must be LOWER than NextBeat Time code
            if (AudioSettings.dspTime +Time.deltaTime >= NextBeat)
                Beat(NextBeat);
            yield return new WaitForEndOfFrame();
        }
        CurrentBeatCount = 0;
        IsPlaying = false;
    }
}