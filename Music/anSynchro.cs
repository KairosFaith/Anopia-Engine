using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
public delegate void BeatFunc(int beatCount, double timeCode);
public static class anSynchro//this is your new update engine
{
    public static BeatFunc PlayOnBeat;// to schedule sounds
    public static TempoData Tempo;
    public static int CurrentBeatCount => _BeatCount;
    static int _BeatCount = 0;
    public static double NextBeat => _NextBeat;
    public static double NextBar => _NextBar;
    static double _NextBeat,_NextBar;
    static MonoBehaviour SynchroHost;
    static Coroutine Instance;
    static bool isPlaying;
    public static void StartSynchro(MonoBehaviour host, double startTime, TempoData tempo, Action<double>OnStart = null)
    {
        Tempo = tempo;
        Instance = host.StartCoroutine(Synchro(startTime));
        OnStart?.Invoke(startTime);
        SynchroHost = host;
    }
    public static void StartSynchro(MonoBehaviour host, TempoData tempo, Action<double> OnStart = null)
    {
        StartSynchro(host, AudioSettings.dspTime + Time.deltaTime, tempo, OnStart);
    }
    public static void StopSynchro()
    {
        SynchroHost.StopCoroutine(Instance);
    }
    public static void ReSync(TempoData tempo)//Must be called using BeatFunc PlayOnBeat
    {
        Tempo = tempo;
    }
    static IEnumerator Synchro(double startTime)
    {
        isPlaying = true;
        _BeatCount = 0;
        _NextBeat = startTime;
        _NextBar = startTime + Tempo.BarLength;
        void Beat(double beatTimeCode)
        {
            _BeatCount++;
            bool reachedBarLine = _BeatCount > Tempo.BeatsPerBar;
            if (reachedBarLine)
                _BeatCount = 1;
            PlayOnBeat?.Invoke(_BeatCount, beatTimeCode);//must call before beat length is calculated in case of resync
            if (reachedBarLine)
                _NextBar += Tempo.BarLength;
            _NextBeat += Tempo.BeatLength;
            //Core.BroadcastEvent("OnBeat", SynchroHost, _BeatCount,beatTimeCode);
        };
        Beat(startTime);
        while (isPlaying)
        {
            yield return new WaitForEndOfFrame();
            if (AudioSettings.dspTime +Time.deltaTime >= _NextBeat)//check 1 frame ahead
                Beat(_NextBeat);
        }
        //Core.BroadcastEvent("OnSynchroStop", SynchroHost);
        _BeatCount = 0;
        isPlaying = false;
    }
}
public abstract class IanMusicMag : IanAudioMag
{
    public TempoData Tempo;
    public abstract SongForm Structure { get; }
    public override IanEvent LoadMag(anDriver driver, AudioMixerGroup output)
    {
        throw new System.NotImplementedException();
    }
}
public enum SongForm
{
    Linear,
    Stem
}
public abstract class IanSong : MonoBehaviour
{
    public abstract void Setup(IanMusicMag mag, AudioMixerGroup output);
    public abstract void StopOnCue(double stopTime);
    public abstract void StopImmediate();
    public abstract void Play(double startTime);
    public abstract void FadeIn(float t);
    public abstract void FadeOut(float t, Action ondone = null);
    public abstract void Mute(bool toMute);
}