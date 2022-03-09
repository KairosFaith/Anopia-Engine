using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(AudioSource))]
public class anMouseoverFeedback : anNarrateSelectable, IPointerEnterHandler, IPointerExitHandler
{
    public bool IgnoreListenerPause;
    public string SoundID;
    public AudioSource Source;
    ClipData Enter, Exit;
    public void Start()
    {
        Source = GetComponent<AudioSource>();
        Source.ignoreListenerPause = IgnoreListenerPause;
        anInteractFeedbackMag mag = (anInteractFeedbackMag)anCore.FetchMag(SoundID);
        Setup(mag);
    }
    public virtual void Setup(anInteractFeedbackMag mag)
    {
        Enter = mag.Enter;
        Exit = mag.Exit;
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        Play(Enter);
    }
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        Play(Exit);
    }
    protected void Play(ClipData data)//TODO need dedicated function?
    {
        AudioClip cliptoPlay = data.Clip;
        if (cliptoPlay != null)
            Source.PlayOneShot(cliptoPlay, data.Gain);
    }
}