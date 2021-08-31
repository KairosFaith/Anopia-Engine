using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SimpleDaredevil : MonoBehaviour,IAnimationEventHandler
{
    AnopiaEventDriver driver;
    NavMeshAgent Agent;
    Animator Anim;
    Vector3 previousPosition;
    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();
        driver = new AnopiaEventDriver(this,DataCore.PlayerChannel);
        driver.LoadEvent("FootstepsConcrete");
        driver.LoadEvent("PunchSwing");
        driver.LoadEvent("PunchImpact");
        driver.LoadEvent("Cloth");
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

    public void OnAnimationEventCallback(AnimationEventType msgtype, params object[] args)
    {
        switch (msgtype)
        {
            case AnimationEventType.ActionPulse:
                Attack();
                break;
            case AnimationEventType.FootstepSound:
                if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
                {
                    int layer = hit.collider.gameObject.layer;
                    string layerName = LayerMask.LayerToName(layer);
                    PlayFootsteps(layerName);
                }
                break;
            case AnimationEventType.FoleySound:
                driver.Play("Cloth");
                break;
            case AnimationEventType.PlayAudioClip:
                AnopiaAudioCore.PlayClipAtPoint(transform.position, (string)args[0], (int)args[1], DataCore.PlayerChannel);
                break;
        }
    }
    void Attack()
    {
        driver.Play("PunchSwing");
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit info, 1, DataCore.EnemyLayerMask))
        {
            SimpleEnemy victim = info.collider.gameObject.GetComponent<SimpleEnemy>();
            victim.GetDamage();
            driver.Play("PunchImpact");
        }
    }
    void PlayFootsteps(string surface)
    {
        string ToPlay = "";
        switch (surface)
        {
            case "concrete":
                ToPlay = "FootstepsConcrete";
                break;
                //add for other surfaces
        }
        driver.Play(ToPlay);
    }
}
