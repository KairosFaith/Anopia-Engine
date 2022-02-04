using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class anDriver : MonoBehaviour
{
    public AudioMixerGroup Output;
    public AudioSource OneShotSource;
    public Dictionary<string, IanEvent> Events = new Dictionary<string, IanEvent>();
    public void SetDriver(string[] soundID)
    {
        foreach (string id in soundID)
            LoadEvent(id);
    }
    void LoadEvent(string SoundID)
    {
        IanEvent e = anCore.NewEvent(this, SoundID, Output);
        Events[SoundID] = e;
        if (e is anOneShotEvent eve)
            eve.audioSource = OneShotSource;
    }
    public void Play(string SoundID, params object[] args)
    {
        Events[SoundID].Play(args);
    }
    public void Stop(string SoundID)
    {
        Events[SoundID].Stop();
    }
    public void StopAll()
    {
        foreach (KeyValuePair<string,IanEvent> eve in Events)
            eve.Value.Stop();
    }
}