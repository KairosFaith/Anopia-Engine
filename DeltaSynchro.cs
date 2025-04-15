using System.Collections;
using UnityEngine;
public class DeltaSynchro : IanSynchro //this is your new update engine
{
	public bool SynchroActive;
    Coroutine _SynchroRoutine;
    double _DeltaMargin => Time.deltaTime;//TODO still experimenting
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
            //double timeGap = NextBeat - AudioSettings.dspTime;
            //if (timeGap <= DeltaMargin)
            if ((NextBeat - AudioSettings.dspTime) <= _DeltaMargin)
            {
                Beat(NextBeat);
                //print(timeGap);
            }
            yield return new WaitForEndOfFrame();
        }
        SynchroActive = false;
    }
}