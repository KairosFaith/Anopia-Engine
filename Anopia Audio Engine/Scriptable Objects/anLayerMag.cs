using System;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "LayerMag", menuName = "AnopiaEngine/LayeredLoops", order = 4)]
public class anLayerMag : IanAudioMag
{
    public LayerData[] Layers;
    public override IanEvent LoadMag(MonoBehaviour host, AudioMixerGroup output)
    {
        throw new NotImplementedException("LayerEvent Coming Soon....");
    }
}
[Serializable] 
public class LayerData : ClipData
{
    public GameObject SourcePrefab;
}