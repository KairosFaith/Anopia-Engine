using UnityEngine.EventSystems;
using UnityEngine.UI;
public class anSliderInteractable : anInteractable
{
    public Slider slider;
    ClipData Drag;
    bool isPressed;
    public override void Setup(anInteractableMag mag)
    {
        base.Setup(mag);
        Drag = mag.Drag;
        slider.onValueChanged.AddListener((f) => OnValueChange());
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!isPressed)
            base.OnPointerEnter(eventData);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        base.OnPointerDown(eventData);
    }
    void OnValueChange()
    {
        //if(Drag.Clip!=null)
        Source.PlayOneShot(Drag.Clip, Drag.Gain * slider.normalizedValue);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        base.OnPointerUp(eventData);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (!isPressed)
            base.OnPointerExit(eventData);
    }
}