using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tCheckpoint : MonoBehaviour
{
    public string CheckpointName;
    private void OnTriggerEnter(Collider other)
    {
        Core.BroadcastEvent("Checkpoint", this, CheckpointName);
    }
}
