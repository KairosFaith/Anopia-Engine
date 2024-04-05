using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(AudioSource))]
public class anMouseoverFeedback : anNarrateSelectable, IPointerEnterHandler, IPointerExitHandler
{
    public bool IgnoreListenerPause;
    public AudioSource Source;
    public anInteractFeedbackMag AudioMag;
    public virtual void Start()
    {
        Source = GetComponent<AudioSource>();
        Source.ignoreListenerPause = IgnoreListenerPause;
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        Source.PlayOneShot(AudioMag.Enter);
    }
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        Source.PlayOneShot(AudioMag.Exit);
    }
}