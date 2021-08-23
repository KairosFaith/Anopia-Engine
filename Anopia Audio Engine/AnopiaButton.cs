using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class AnopiaButton : Button
{
    public string SoundID;
    [Range(-1, 1)]
    public float Pan;
    AudioSource Source;
    ClipData Enter;
    ClipData Down;
    ClipData Exit; 
    public override void OnPointerEnter(PointerEventData eventData)
    {
        Source.PlayOneShot(Enter);
        base.OnPointerEnter(eventData);
    }
    public void AddListener(UnityAction call, AudioMixerGroup output)
    {
        onClick.AddListener(call);
        Source = AnopiaAudioCore.NewStereoSource(this, output);
        onClick.AddListener(() => Source.PlayOneShot(Down));
        Source.ignoreListenerPause = true;
        Source.panStereo = Pan;
        ButtonMag mag = (ButtonMag)AnopiaAudioCore.FetchMag(SoundID);
        Enter = mag.Enter;
        Down = mag.Down;
        Exit = mag.Exit;
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        Source.PlayOneShot(Exit);
        base.OnPointerExit(eventData);
    }
}
