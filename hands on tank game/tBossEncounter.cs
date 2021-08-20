using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tBossEncounter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Core.BroadcastEvent("FinalEncounter", this);
    }
}
