using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
public static partial class anCore
{
    public static anSourcerer Setup2DSource(AudioClip clip, AudioMixerGroup channel, float pan)
    {
        GameObject g = new GameObject(clip.name);
        anSourcerer a = g.AddComponent<anSourcerer>();
        a.Channel = channel;
        a.audioSource.clip = clip;
        a.audioSource.panStereo = pan;
        return a;
    }
    public static anSourcerer SetupTrackSource(TrackData track)
    {
        return Setup2DSource(track.Clip, track.Channel, track.Pan);
    }
    /// <summary>
    /// plays a track at the given time, and deletes the source after the clip has finished playing
    /// </summary>
    /// <param name="track"></param>
    /// <param name="startTime"></param>
    /// <returns></returns>
    public static anSourcerer PlayTrackScheduled(TrackData track, double startTime)
    {
        anSourcerer s = SetupTrackSource(track);
        s.PlayScheduled(startTime);
        s.DeleteAfterTime(startTime + track.Clip.length);
        return s;
    }
}