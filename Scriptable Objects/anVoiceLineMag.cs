using System;
using UnityEngine;
[CreateAssetMenu(fileName = "VoiceLineMag", menuName = "AnopiaEngine/VoiceLineMag")]
public class anVoiceLineMag : anSpeechMag
{
    public VoiceLineData[] SpeechLines;
    protected override void InitSpeechBank()
    {
        base.InitSpeechBank();
        foreach (VoiceLineData v in SpeechLines)
            SpeechBank.Add(v.Line, v.Clip);
    }
}
[Serializable]
public class VoiceLineData
{
    public string Line;
    public AudioClip Clip;
}