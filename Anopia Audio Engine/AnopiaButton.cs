using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[RequireComponent(typeof(AudioSource))]
public class AnopiaButton : Button
{
    public string SoundID;
    AudioSource Source;
    ClipData Enter;
    ClipData Down;
    ClipData Exit;
    protected override void Start()
    {
        Source = GetComponent<AudioSource>();
        Source.ignoreListenerPause = true;
        onClick.AddListener(() => Source.PlayOneShot(Down));
        anButtonMag mag = (anButtonMag)AnopiaAudioCore.FetchMag(SoundID);
        Enter = mag.Enter;
        Down = mag.Down;
        Exit = mag.Exit;
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        Source.PlayOneShot(Enter);
        base.OnPointerEnter(eventData);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        Source.PlayOneShot(Exit);
        base.OnPointerExit(eventData);
    }
}
