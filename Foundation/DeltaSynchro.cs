using System.Collections;
using UnityEngine;
public class DeltaSynchro : IanSynchro //this is your new update engine
{
	public bool SynchroActive;
    Coroutine _SynchroRoutine;
    public double DeltaMargin => Time.deltaTime * 11;//TODO still experimenting
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
        Beat(startTime);
        while(SynchroActive)
            if ((NextBeat - AudioSettings.dspTime) <= DeltaMargin)
                Beat(NextBeat);
            else
                yield return new WaitForEndOfFrame();
        SynchroActive = false;
    }
    void Beat(double beatTimeCode)
    {
        if (CurrentBeatCount == Tempo.BeatsPerBar)
        {
            CurrentBeatCount = 1;
            CurrentBar = beatTimeCode;
        }
        else
            CurrentBeatCount++;
        CurrentBeat = NextBeat;
        NextBeat += Tempo.BeatLength;
        PlayOnBeat?.Invoke(CurrentBeatCount, beatTimeCode);
    }
}