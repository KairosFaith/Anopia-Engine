using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(AudioSource))]
public class anInteractable : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public string SoundID;
    public AudioSource Source;
    ClipData Enter, Down, Up, Exit;
    void Start()
    {
        Source = GetComponent<AudioSource>();
        Source.ignoreListenerPause = true;
        anInteractableMag mag = (anInteractableMag)anCore.FetchMag(SoundID);
        Setup(mag);
    }
    public virtual void Setup(anInteractableMag mag)
    {
        Enter = mag.Enter;
        Down = mag.Down;
        Up = mag.Up;
        Exit = mag.Exit;
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        AudioClip cliptoPlay = Enter.Clip;
        if (cliptoPlay != null)
            Source.PlayOneShot(cliptoPlay, Enter.Gain);
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        AudioClip cliptoPlay = Down.Clip;
        if (cliptoPlay != null)
            Source.PlayOneShot(cliptoPlay, Down.Gain);
    }
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        AudioClip cliptoPlay = Up.Clip;
        if (cliptoPlay != null)
            Source.PlayOneShot(cliptoPlay, Up.Gain);
    }
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        AudioClip cliptoPlay = Exit.Clip;
        if (cliptoPlay != null)
            Source.PlayOneShot(cliptoPlay, Exit.Gain);
    }
    //TODO common play function?
}