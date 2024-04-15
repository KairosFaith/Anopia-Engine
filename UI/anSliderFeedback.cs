using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class anSliderFeedback : anMouseoverFeedback, IPointerDownHandler, IPointerUpHandler
{
    public Slider slider;
    bool _InSlider, _IsDown;
    public override void Start()
    {
        base.Start();
        slider.onValueChanged.AddListener((f) => OnValueChange());
    }
    void OnValueChange()
    {
        Source.PlayOneShot(AudioMag.Interact, slider.normalizedValue);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        _InSlider = true;
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        _IsDown = true;
        if (!_IsDown)
            Source.PlayOneShot(AudioMag.Down);
    }
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        _IsDown = false;
        if (_InSlider)
            Source.PlayOneShot(AudioMag.Up);
    }
    override public void OnPointerExit(PointerEventData eventData)
    {
        _InSlider = false;
        if (!_IsDown)
            base.OnPointerExit(eventData);
    }
}