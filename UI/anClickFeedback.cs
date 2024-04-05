using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(AudioSource))]
public class anClickFeedback : anMouseoverFeedback, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Source.PlayOneShot(AudioMag.Interact);
    }
}