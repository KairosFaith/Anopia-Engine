using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class anDriver : MonoBehaviour
{
    public AudioMixerGroup Output;
    public anSourcerer SourcePrefab;
    [HideInInspector]
    public anSourcerer OneShotSource;
    public Dictionary<string, IanEvent> Events = new Dictionary<string, IanEvent>();
    public void SetDriver(params string[] soundID)
    {
    //    foreach (string id in soundID)
    //        LoadEvent(id);
    }
    public void Play(string SoundID, params object[] args)
    {
        if (Events.TryGetValue(SoundID, out IanEvent value))
            value.Play(args);
        else
            throw new System.Exception(SoundID+"not loaded in driver");
    }
    public void SetParameter(string SoundID,string paramName, float value)
    {
        if (Events.TryGetValue(SoundID, out IanEvent existing))
            existing.SetParameter(paramName,value);
        else
            throw new System.Exception(SoundID + "not loaded in driver");
    }
    public void Stop(string SoundID)
    {
        if (Events.TryGetValue(SoundID, out IanEvent value))
            value.Stop();
        else
            throw new System.Exception(SoundID + "not loaded in driver");
    }
    public void StopAll()
    {
        foreach (KeyValuePair<string,IanEvent> eve in Events)
            eve.Value.Stop();
    }
}