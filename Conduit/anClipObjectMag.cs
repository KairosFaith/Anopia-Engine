using UnityEngine;
[CreateAssetMenu(fileName = "ClipObjectMag", menuName = "AnopiaEngine/ClipObjectMag", order = 2)]
public class anClipObjectMag : anClipMag
{
    [Range(0, 1)]
    public float MinPitch = 1;
    [Range(1, 2)]
    public float MaxPitch = 1;
}