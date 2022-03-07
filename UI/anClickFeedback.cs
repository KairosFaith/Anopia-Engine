using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(AudioSource))]
public class anClickFeedback : anNarrateSelectable, IPointerEnterHandler,IPointerClickHandler,IPointerExitHandler
{
    public string SoundID;
    public AudioSource Source;
    ClipData Enter, Interact, Exit;
    void Start()
    {
        Source = GetComponent<AudioSource>();
        Source.ignoreListenerPause = true;
        anInteractableMag mag = (anInteractableMag)anCore.FetchMag(SoundID);
        Setup(mag);
    }
    public void Setup(anInteractableMag mag)
    {
        Enter = mag.Enter;
        Interact = mag.Interact;
        Exit = mag.Exit;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Play(Enter);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Play(Interact);
    }
    public void OnPointerExit(PointerEventData eventData)
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