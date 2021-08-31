using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(AudioSource))]
public class AnopiaSlider : Slider
{
    public string SoundID;
    AudioSource Source;
    ClipData Enter;
    ClipData Down;
    ClipData Drag;
    ClipData Up;
    ClipData Exit;
    protected override void Start()
    {
        Source = GetComponent<AudioSource>();
        onValueChanged.AddListener((f) => Source.PlayOneShot(Drag.Clip, Drag.Gain*normalizedValue));
        Source.ignoreListenerPause = true;
        anSliderMag mag = (anSliderMag)AnopiaAudioCore.FetchMag(SoundID);
        Enter = mag.Enter;
        Down = mag.Down;
        Drag = mag.Drag;
        Up = mag.Up;
        Exit = mag.Exit;
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        Source.PlayOneShot(Enter);
        base.OnPointerEnter(eventData);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        Source.PlayOneShot(Down);
        base.OnPointerDown(eventData);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        Source.PlayOneShot(Up);
        base.OnPointerUp(eventData);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        Source.PlayOneShot(Exit);
        base.OnPointerExit(eventData);
    }
}
