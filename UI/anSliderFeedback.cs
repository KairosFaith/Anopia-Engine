using UnityEngine.UI;
using UnityEngine.EventSystems;
public class anSliderFeedback : anMouseoverFeedback, IPointerDownHandler, IPointerUpHandler
{
    public Slider slider;
    bool _IsDown;
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
        if (!_IsDown)
            base.OnPointerEnter(eventData);
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        _IsDown = true;
            Source.PlayOneShot(AudioMag.Down);
    }
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        _IsDown = false;
            Source.PlayOneShot(AudioMag.Up);
    }
    override public void OnPointerExit(PointerEventData eventData)
    {
        if (!_IsDown)
            base.OnPointerExit(eventData);
    }
}