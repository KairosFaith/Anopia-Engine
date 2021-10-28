using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AnopiaEventDriver : MonoBehaviour
{
    MonoBehaviour _Host;
    AudioMixerGroup _Output;
    public AnopiaSourcerer OneShotSource;
    public Dictionary<string, IanEvent> Events = new Dictionary<string, IanEvent>();
    public AnopiaEventDriver(MonoBehaviour host, AudioMixerGroup output, params string[] IDs)
    {
        _Host = host;
        _Output = output;
        foreach (string id in IDs)
            LoadEvent(id);
    }
    void LoadEvent(string SoundID)
    {
        IanEvent e = AnopiaAudioCore.NewEvent(_Host, SoundID, _Output);
        Events[SoundID] = e;
        if (e is anOneShotEvent eve)
            eve.audioSource = OneShotSource.audioSource;
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