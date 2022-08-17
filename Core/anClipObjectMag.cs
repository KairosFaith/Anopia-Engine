using System;
using UnityEngine;
[CreateAssetMenu(fileName = "ClipObjectMag", menuName = "AnopiaEngine/ClipObjectMag", order = 2)]
public class anClipObjectMag : anClipMag
{
    public AudioSource SourcererPrefab;
    public AudioSource PlayClipAtPoint(Vector3 position,Action<anSourcerer> beforePlay = null)
    {
        AudioSource a = Instantiate(SourcererPrefab, position, Quaternion.identity);
        AudioClip c = Randomise(out float gain);
        beforePlay?.Invoke(a.GetComponent<anSourcerer>());
        a.PlayOneShot(c, gain);
        Destroy(a.gameObject, c.length);
        return a;
    }
}