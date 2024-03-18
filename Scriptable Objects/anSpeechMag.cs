using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "SpeechMag", menuName = "AnopiaEngine/SpeechMag")]
public class anSpeechMag : anClipMag
{
    protected Dictionary<string, AudioClip> SpeechBank
    {
        get
        {
            if(_SpeechBank == null)
                InitSpeechBank();
            return _SpeechBank;
        }
    }
    Dictionary<string, AudioClip> _SpeechBank;
    protected virtual void InitSpeechBank()
    {
        _SpeechBank = new Dictionary<string, AudioClip>();
        foreach (AudioClip c in Data)
            _SpeechBank.Add(c.name, c);
    }
    public void PlaySpeech(AudioSource source, string msg)
    {
        source.Stop();
        if (SpeechBank.TryGetValue(msg, out AudioClip clip))
            source.PlayOneShot(clip);
    }
    public anSourcerer[] PlaySpeechSequence(AudioMixerGroup channel, double timeCode, string[] msgs)
    {
        List<anSourcerer> sources = new List<anSourcerer>();
        foreach (string msg in msgs)
            if (SpeechBank.TryGetValue(msg, out AudioClip clip))
            {
                anSourcerer a = anCore.Setup2DSource(clip, channel);
                a.audioSource.PlayScheduled(timeCode);
                sources.Add(a);
                timeCode += clip.length;
            }
        foreach (anSourcerer a in sources)
            a.DeleteAfterTime(timeCode);
        return sources.ToArray();
    }
    [Serializable]
    public class VoiceLineData
    {
        public string Line;
        public AudioClip Clip;
    }
}