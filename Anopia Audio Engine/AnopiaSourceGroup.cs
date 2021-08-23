using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
public class AnopiaSourceGroup : MonoBehaviour
{
    public List<AudioSource> Sources = new List<AudioSource>();
    public void InvokeSources(string MusicID, AudioMixerGroup[] output)
    {//for music
        ClipMag mag = (ClipMag)AnopiaAudioCore.FetchMag(MusicID);
        ClipData[] clips = mag.Data;
        for(int i = 0; i < clips.Length; i++)
        {
            int b = (i < output.Length) ? i : output.Length - 1;
            AudioSource a = AnopiaAudioCore.NewStereoSource(this, output[b]);
            ClipData c = clips[i];
            a.clip = c.Clip;
            a.volume = c.Gain;
            a.loop = true;
            Sources.Add(a);
        }
    }
    public void Play()
    {
        foreach (AudioSource a in Sources)
            a.Play();
    }
    public void Pause()
    {
        foreach (AudioSource a in Sources)
            a.Pause();
    }
    public void Resume()
    {
        foreach (AudioSource a in Sources)
            a.UnPause();
    }
    public void FadeIn(float fadeTime, Action ondone = null)
    {
        foreach (AudioSource a in Sources)
        {
            float defaultGain = a.volume;
            Action<float> ChangeValue = (newValue) =>
            {
                a.volume = newValue;
            };
            StartCoroutine(FadeValue(fadeTime, a.volume, defaultGain, ChangeValue, ondone));
        }
    }
    public void FadeOut(float fadeTime, Action ondone = null)
    {
        foreach (AudioSource a in Sources)
        {
            Action<float> ChangeValue = (newValue) =>
            {
                a.volume = newValue;
            };

            Action destroy = () =>
            {
                Destroy(a.gameObject);
                Sources.Remove(a);
                if (Sources.Count == 0)
                    Destroy(gameObject);
            };
            ondone += destroy;
            StartCoroutine(FadeValue(fadeTime, a.volume, 0, ChangeValue, ondone));
        }
    }
    IEnumerator FadeValue(float fadeTime, float startingValue, float targetValue, Action<float> ChangeValue, Action ondone = null)
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