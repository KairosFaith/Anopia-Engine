using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(AudioSource))]
public class anPointerInteractFeedback : anNarrateSelectable, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public string SoundID;
    public AudioSource Source;
    ClipData Enter, Down, Up, Exit;
    public void Start()
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
        Play(Enter);
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Play(Down);
    }
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        Play(Up);
    }
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        Play(Exit);
    }
    void Play(ClipData data)
    {
        AudioClip cliptoPlay = data.Clip;
        if (cliptoPlay != null)
            Source.PlayOneShot(cliptoPlay, data.Gain);
    }
}