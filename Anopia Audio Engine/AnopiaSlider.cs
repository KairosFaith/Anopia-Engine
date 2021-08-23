using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
public class AnopiaSlider : Slider
{
    public string SoundID;
    [Range(-1,1)]
    public float Pan;
    AudioSource Source;
    ClipData Enter;
    ClipData Down;
    ClipData Drag;
    ClipData Up;
    ClipData Exit;
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
    public void AddListener(UnityAction<float> call, AudioMixerGroup output)
    {
        onValueChanged.AddListener(call);
        Source = AnopiaAudioCore.NewStereoSource(this,output);
        onValueChanged.AddListener((f) => Source.PlayOneShot(Drag));
        Source.ignoreListenerPause = true;
        Source.panStereo = Pan;
        SliderMag mag = (SliderMag)AnopiaAudioCore.FetchMag(SoundID);
        Enter = mag.Enter;
        Down = mag.Down;
        Drag = mag.Drag;
        Up = mag.Up;
        Exit = mag.Exit;
    }
}
