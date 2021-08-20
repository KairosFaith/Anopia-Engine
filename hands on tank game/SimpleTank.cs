using UnityEngine;
public class SimpleTank : MonoBehaviour
{
    public float MaxVelocity;
    public float DriveForce;
    public float RotSpeed;
    [Range(0, 1)]
    public float MinPitch = 1;
    [Range(1, 2)]
    public float MaxPitch = 1;
    [Range(0,1)]
    public float PowerUpDistortion = 0.7f;
    FlexSourceHandler WeaponSource;
    FlexSourceHandler EngineIdle;
    FlexSourceHandler EngineDrive;
    FlexSourceHandler EngineRotate;
    void Start()
    {
        WeaponSource = FlexEngine.NewHandler(this, OutputPan.Point, tAudioEventManager.Instance.PlayerChannel, SourceID.PlayerWeaponSource);
        WeaponSource.AssignClipData(AudioFileID.TankWeapon2, 1);
        EngineIdle = FlexEngine.NewHandler(this, OutputPan.Point, tAudioEventManager.Instance.PlayerChannel, SourceID.DefaultSoundObject);
        EngineDrive = FlexEngine.NewHandler(this, OutputPan.Point, tAudioEventManager.Instance.PlayerChannel, SourceID.DefaultSoundObject);
        EngineRotate = FlexEngine.NewHandler(this, OutputPan.Point, tAudioEventManager.Instance.PlayerChannel, SourceID.DefaultSoundObject);
        UpdateAudio(0, 0);
        EngineIdle.Play(AudioFileID.TankEngine, 0);
        EngineDrive.Play(AudioFileID.TankEngine, 1);
        EngineRotate.Play(AudioFileID.TankEngine, 2);
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
            void settings(FlexSourceHandler h)
            {
                h.Pitch = Random.Range(MinPitch, MaxPitch);
            }
            WeaponSource.PlayClipAtPoint(AudioFileID.TankWeapon1, settings);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            WeaponSource.PlayOneShot(AudioFileID.TankWeapon2, 0);
            WeaponSource.Source.Play();
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            WeaponSource.PlayOneShot(AudioFileID.TankWeapon2, 0);
            WeaponSource.Source.Stop();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            WeaponSource.Distortion = PowerUpDistortion;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            WeaponSource.Distortion = 0;
        }
    }
    public AudioCurve PitchCurve;
    public AudioCurve DriveVolumeCurve;
    void UpdateAudio(float x, float y)
    {
        EngineIdle.Pitch = PitchCurve.Evaluate(x);
        EngineDrive.Volume = DriveVolumeCurve.Evaluate(x);
        EngineRotate.Volume = y;
    }
}