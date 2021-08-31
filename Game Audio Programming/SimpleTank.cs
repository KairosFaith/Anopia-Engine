using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SimpleTank : MonoBehaviour
{
    anClipEffectsEvent Weapon1;
    anADSREvent Weapon2;
    AnopiaSourcerer[] Engine;

    public AudioCurve PitchCurve;
    public AudioCurve DriveVolumeCurve;

    public float MaxVelocity;
    public float DriveForce;
    public float RotSpeed;
    void Start()
    {
        AudioMixerGroup output = DataCore.PlayerChannel;
        Weapon1 = (anClipEffectsEvent)AnopiaAudioCore.NewEvent(this, "TankWeapon1", output);
        Weapon2 = (anADSREvent)AnopiaAudioCore.NewEvent(this, "TankWeapon2", output);
        Engine = AnopiaAudioCore.SetLayers(this, "TankEngine", output);
    }
    void Update()
    {
        float x = Input.GetAxis("Vertical");
        if (x > 0 || x < 0)
        {
            Vector3 dir = new Vector3(x, 0);
            transform.Translate(dir.normalized * Time.deltaTime * DriveForce);
        }
        float y = Input.GetAxis("Horizontal");
        if (y > 0 || y < 0)
        {
            Vector3 rot = new Vector3(0, y);
            transform.Rotate(rot.normalized * Time.deltaTime * RotSpeed);
        }
        UpdateAudio(Mathf.Abs(x), Mathf.Abs(y));
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Weapon1.Play();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Weapon2.Play();
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            Weapon2.Stop();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Weapon2.Sourcerer.Distortion = 0.7f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Weapon2.Sourcerer.Distortion = 0.3f;
        }
    }
    void UpdateAudio(float x, float y)
    {
        Engine[0].Pitch = PitchCurve.Evaluate(x);
        Engine[1].Volume = DriveVolumeCurve.Evaluate(x);
        Engine[2].Volume = y;
    }
}