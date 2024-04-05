using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class anSliderFeedback : anMouseoverFeedback, IPointerDownHandler, IPointerUpHandler
{
    public Slider slider;
    public override void Start()
    {
        base.Start();
        slider.onValueChanged.AddListener((f) => OnValueChange());
    }
    void OnValueChange()
    {
        Source.PlayOneShot(AudioMag.Interact, slider.normalizedValue);
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Source.PlayOneShot(AudioMag.Down);
    }
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        Source.PlayOneShot(AudioMag.Up);
    }
}