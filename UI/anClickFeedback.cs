using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(AudioSource))]
public class anClickFeedback : anMouseoverFeedback, IPointerClickHandler
{
    ClipData Interact;
    public override void Setup(anInteractFeedbackMag mag)
    {
        base.Setup(mag);
        Interact = mag.Interact;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Play(Interact);
    }
}