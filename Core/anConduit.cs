using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
public static partial class anCore
{
    public static anSourcerer Setup2DSource(AudioClip clip, AudioMixerGroup channel)
    {
        GameObject g = new GameObject(clip.name);
        anSourcerer a = g.AddComponent<anSourcerer>();
        a.SetChannel(channel);
        a.audioSource.clip = clip;
        return a;
    }
    //public static anSourcerer SetupSource(Vector3 position, anSourcerer prefab, AudioMixerGroup output, AudioClip clip, float volume)
    //{
    //    anSourcerer s = UnityEngine.Object.Instantiate(prefab, position, Quaternion.identity);
    //    AudioSource a = s.audioSource;
    //    a.outputAudioMixerGroup = output;
    //    a.clip = clip;
    //    a.volume = volume;
    //    return s;
    //}
    //public static anSourcerer PlayClipAtPoint(Vector3 position, AudioClip clip, float volume, AudioMixerGroup output, anSourcerer prefab, Action<anSourcerer> setup = null, Action OnDone = null)
    //{
    //    anSourcerer s = SetupSource(position, prefab, output, clip, volume);
    //    AudioSource a = s.audioSource;
    //    setup?.Invoke(s);
    //    a.Play();
    //    s.DeleteWhenDone(OnDone);
    //    return s;
    //}
    public static anSourcerer PlayClipScheduled(AudioClip clip, double startTime, AudioMixerGroup output)
    {
        anSourcerer s = Setup2DSource(clip, output);
        s.audioSource.PlayScheduled(startTime);
        s.DeleteAfterTime(startTime + clip.length);
        return s;
    }
    public static IEnumerator FadeValue(float fadeTime, float startingValue, float targetValue, Action<float> ChangeValue, Action ondone = null)
    {
        for (float lerp = 0; lerp < 1; lerp += Time.unscaledDeltaTime / fadeTime)
        {
            float newValue = Mathf.Lerp(startingValue, targetValue, lerp);
            ChangeValue(newValue);
            yield return new WaitForEndOfFrame();
        }
        ondone?.Invoke();
    }
}