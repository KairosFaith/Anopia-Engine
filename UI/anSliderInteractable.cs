using UnityEngine.UI;
public class anSliderInteractable : anInteractable
{
    public Slider slider;
    ClipData Drag;
    public override void Setup(anInteractableMag mag)
    {
        base.Setup(mag);
        Drag = mag.Interact;
        slider.onValueChanged.AddListener((f) => OnValueChange());
    }
    void OnValueChange()
    {
        Source.PlayOneShot(Drag.Clip, Drag.Gain * slider.normalizedValue);
    }
}