using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "LayerMag", menuName = "AnopiaEngine/LayerMag", order = 4)]
public class anLayerMag : IanAudioMag
{
    public AudioClip[] Layers;
    public LayerAutomationData[] AutomationData;
}