using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    public int HP = 3;

    public AudioFileID DamageSound;
    public AudioFileID DeathSound;
    FlexSourceHandler MainSource;
    private void Start()
    {
        MainSource = FlexEngine.NewHandler(this, OutputPan.Point, vAudioEventManager.Instance.EnemyChannel, SourceID.DefaultEnemy);
    }
    public void GetDamage()
    {
        HP--;
        MainSource.PlayOneShot(DamageSound);
        if (HP <= 0)
        {
            MainSource.PlayClipAtPoint(DeathSound);
            Destroy(gameObject);
        }
    }
}