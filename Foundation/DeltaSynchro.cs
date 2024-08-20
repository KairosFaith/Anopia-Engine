using System.Collections;
using UnityEngine;
public class DeltaSynchro : IanSynchro //this is your new update engine
{
	public bool SynchroActive;
    static Coroutine _SynchroRoutine;
    static double DeltaMargin => Time.deltaTime;//TODO still experimenting
    public override void StartSynchro(double startTime)
    {
        StopSynchro();
        _SynchroRoutine = Instance.StartCoroutine(Synchro(startTime));
    }
    public override void StopSynchro()
    {
        SynchroActive = false;
        if(_SynchroRoutine!=null)
            Instance.StopCoroutine(_SynchroRoutine);
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
            if ((NextBeat - AudioSettings.dspTime) <= DeltaMargin)
                Beat(NextBeat);
            yield return new WaitForEndOfFrame();
        }
        SynchroActive = false;
    }
}