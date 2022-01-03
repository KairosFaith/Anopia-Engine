using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(AudioSource))]
public class anSlider : Slider
{
    public string SoundID;
    AudioSource Source;
    ClipData Enter;
    ClipData Down;
    ClipData Drag;
    ClipData Up;
    ClipData Exit;
    bool isPressed;
    protected override void Start()
    {
        Source = GetComponent<AudioSource>();
        onValueChanged.AddListener((f) => Source.PlayOneShot(Drag.Clip, Drag.Gain*normalizedValue));
        Source.ignoreListenerPause = true;
        anSliderMag mag = (anSliderMag)anCore.FetchMag(SoundID);
        Enter = mag.Enter;
        Down = mag.Down;
        Drag = mag.Drag;
        Up = mag.Up;
        Exit = mag.Exit;
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!isPressed)
            Source.PlayOneShot(Enter.Clip,Enter.Gain);
        base.OnPointerEnter(eventData);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        Source.PlayOneShot(Down.Clip, Down.Gain);
        isPressed = true;
        base.OnPointerDown(eventData);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        Source.PlayOneShot(Up.Clip,Up.Gain);
        isPressed = false;
        base.OnPointerUp(eventData);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if(!isPressed)
        Source.PlayOneShot(Exit.Clip,Exit.Gain);
        base.OnPointerExit(eventData);
    }
}
