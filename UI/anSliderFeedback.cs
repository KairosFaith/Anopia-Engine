using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class anSliderFeedback : anMouseoverFeedback, IPointerDownHandler, IPointerUpHandler
{
    public Slider slider;
    AudioClip Down, Up, Drag;
    public override void Setup(anInteractFeedbackMag mag)
    {
        base.Setup(mag);
        Down = mag.Down;
        Up = mag.Up;
        Drag = mag.Interact;
        slider.onValueChanged.AddListener((f) => OnValueChange());
    }
    void OnValueChange()
    {
        Source.PlayOneShot(Drag, slider.normalizedValue);
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Play(Down);
    }
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        Play(Up);
    }
}