using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    IAnimationEventHandler _handler;
    // Start is called before the first frame update
    void Start()
    {
        _handler = this.GetComponent<IAnimationEventHandler>();
        if (_handler == null) _handler = this.GetComponentInParent<IAnimationEventHandler>();

        if (_handler == null)
            throw new System.Exception("GameObject does not contain a IAnimationEventHandler!");
    }
    public void OnAnimationEvent(AnimationEventType msgtype)
    {
        _handler.OnAnimationEventCallback(msgtype);
    }
    public void AnimationSpecifyClip(string ClipIDKey)//for triggering specified clips to sync to animation
    {
        string[] separators = new string[] {" "};
        string[] args = ClipIDKey.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
        if (args.Length > 1)
            _handler.OnAnimationEventCallback(AnimationEventType.PlayAudioClip, args[0], int.Parse(args[1]));
        else
            _handler.OnAnimationEventCallback(AnimationEventType.PlayAudioClip, args[0]);
    }
}
public interface IAnimationEventHandler
{
    void OnAnimationEventCallback(AnimationEventType msgtype, params object[] args);
}
public enum AnimationEventType
{
    ActionStart,
    ActionDone,
    ActionPulse,
    ActionPulseBeta,
    ActionFeedback,
    ActionFeedbackBeta,
    //new for audio
    PlayAudioClip,

    FoleySound,
    FootstepSound,
    BreathSound,
    //Add more if needed
}