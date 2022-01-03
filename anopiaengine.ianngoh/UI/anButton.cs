using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[RequireComponent(typeof(AudioSource))]
public class anButton : Button
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
        onClick.AddListener(() => Source.PlayOneShot(Down.Clip,Down.Gain));
        anButtonMag mag = (anButtonMag)anCore.FetchMag(SoundID);
        Enter = mag.Enter;
        Down = mag.Down;
        Exit = mag.Exit;
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        Source.PlayOneShot(Enter.Clip,Enter.Gain);
        base.OnPointerEnter(eventData);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        Source.PlayOneShot(Exit.Clip,Exit.Gain);
        base.OnPointerExit(eventData);
    }
}
