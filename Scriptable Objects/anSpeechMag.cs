using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public virtual void InitSpeechBank()
    {
        _SpeechBank = new Dictionary<string, AudioClip>();
        foreach (AudioClip c in Clips)
            _SpeechBank.Add(c.name, c);
    }
    public void PlaySpeech(anSourcerer sourcerer, string msg)
    {
        PlaySpeech(sourcerer.audioSource, msg);
    }
    public AudioClip PlaySpeech(AudioSource source, string msg)
    {
        source.Stop();
        if (SpeechBank.TryGetValue(msg, out AudioClip clip))
            source.PlayOneShot(clip);
        return clip;
    }
    public void PlaySpeechSequence(anSourcerer sourcerer, params string[] msgs)
    {
        sourcerer.StartCoroutine(SequenceSpeech(sourcerer.audioSource, msgs));
    }
    IEnumerator SequenceSpeech(AudioSource source, params string[] msgs)
    {
        foreach (string msg in msgs)
        {
            PlaySpeech(source, msg);
            yield return new WaitUntil(() => !source.isPlaying);
        }
    }
}