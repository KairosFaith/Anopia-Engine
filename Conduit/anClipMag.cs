using UnityEngine;
[CreateAssetMenu(fileName = "ClipMag", menuName = "AnopiaEngine/ClipMag", order = 1)]
public class anClipMag : ScriptableObject
{
    public ClipData[] Data;
    [Range(0, 1)]
    public float MinRandomVolume = 1;
}