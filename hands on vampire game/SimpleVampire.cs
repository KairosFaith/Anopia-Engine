using UnityEngine;
using UnityEngine.AI;
public class SimpleVampire : MonoBehaviour, IAnimationEventHandler
{
    public LayerMask EnemyLayer;
    FlexSourceHandler Source;
    NavMeshAgent Agent;
    Animator Anim;
    Vector3 previousPosition;
    public void OnAnimationEventCallback(AnimationEventType msgtype, params object[] args)
    {
        switch (msgtype)
        {
            case AnimationEventType.ActionPulse:
                Attack();
                break;
            case AnimationEventType.FootstepSound:
                if (Physics.Raycast(transform.position,Vector3.down, out RaycastHit hit))
                {
                    int layer = hit.collider.gameObject.layer;
                    string layerName = LayerMask.LayerToName(layer);
                    PlayFootsteps(layerName);
                }
                break;
            case AnimationEventType.FoleySound:
                Source.PlayOneShot(AudioFileID.Cloth);
                break;
            case AnimationEventType.BreathSound:
                Source.PlayOneShot(AudioFileID.Breath);
                break;
            case AnimationEventType.PlayAudioClip:
                Source.PlayOneShot((AudioFileID)args[0],(int)args[1]);
                break;
        }
    }
    void PlayFootsteps(string surface)
    {
        AudioFileID ToPlay = 0;
        switch (surface)
        {
            case "concrete":
                ToPlay = AudioFileID.FootstepsConcrete;
                break;
                //add for other surfaces
        }
        Source.PlayOneShot(ToPlay);
    }
    void Start()
    {
        Source = FlexEngine.NewHandler(this, OutputPan.Point, vAudioEventManager.Instance.PlayerChannel, SourceID.DefaultSoundObject);
        Agent = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();
    }
    void Attack()
    {
        Source.PlayOneShot(AudioFileID.PunchSwing);
        if(Physics.Raycast(transform.position,transform.forward,out RaycastHit info, 1, EnemyLayer))
        {
            SimpleEnemy victim = info.collider.gameObject.GetComponent<SimpleEnemy>();
            victim.GetDamage();
            Source.PlayClipAtPoint(AudioFileID.PunchSwing,victim.transform.position);
        }
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100))
            {
                Agent.isStopped = false;
                Agent.destination = hit.point;
            }
        }
        Vector3 curPos = transform.position;
        Vector3 prevPos = previousPosition;
        previousPosition = curPos;
        bool isWalking = (prevPos - curPos).magnitude > Mathf.Epsilon;
        Anim.SetBool("isWalking", isWalking);
        if (Input.GetButtonDown("Fire2"))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100))
            {
                transform.LookAt(hit.point);
            }
            Agent.isStopped = true;
            Anim.SetTrigger("attack");
        }
    }
}