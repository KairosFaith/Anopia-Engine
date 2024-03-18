using System.Collections;
using System.Collections.Generic;
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
