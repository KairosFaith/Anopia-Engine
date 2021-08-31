using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AnopiaEventDriver
{
    MonoBehaviour _Host;
    AudioMixerGroup _Output;
    AnopiaSourcerer TransientSourcerer;
    Dictionary<string, IanEvent> _Events = new Dictionary<string, IanEvent>();
    public AnopiaEventDriver(MonoBehaviour host,AudioMixerGroup output)
    {
        _Host = host;
        _Output = output;
    }
    public void LoadEvent(string SoundID)
    {
        IanEvent e = AnopiaAudioCore.NewEvent(_Host, SoundID, _Output);
        _Events[SoundID] = e;
        if (e is anOneShotEvent eve)
        {
            if (TransientSourcerer == null)
                TransientSourcerer = AnopiaAudioCore.NewPointSource(_Host, eve.SourcePrefab, _Output);
            eve.Sourcerer = TransientSourcerer;
        }
    }
    public void Play(string SoundID, params object[] args)
    {
        _Events[SoundID].Play(args);
    }
    public void Stop(string SoundID)
    {
        _Events[SoundID].Stop();
    }
}
