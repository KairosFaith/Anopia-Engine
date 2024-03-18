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
    public static anSourcerer Setup2DLoopSource(AudioClip clip, AudioMixerGroup channel)//TODO how about just merge the functions???
    {
        anSourcerer a = Setup2DSource(clip, channel);
        a.audioSource.loop = true;
        return a;
    }
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