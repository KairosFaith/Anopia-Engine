using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    public int HP = 3;
    IanEvent DamageSound;
    anClipEffectsEvent DeathSound;
    private void Start()
    {
        DamageSound = AnopiaAudioCore.NewEvent(this, "EnemyDamage", DataCore.EnemyChannel);
        DeathSound = (anClipEffectsEvent)AnopiaAudioCore.NewEvent(this, "EnemyDeath", DataCore.EnemyChannel);
    }
    public void GetDamage()
    {
        HP--;
        DamageSound.Play();
        if (HP <= 0)
        {
            DeathSound.Play();
            Destroy(gameObject);
        }
    }
}
