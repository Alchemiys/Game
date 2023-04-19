using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public int hitpoint = 10;
    public int maxHitpoing = 10;
    public float pushRecoverySpeed = 0.2f;

    // imunity
    protected float imuneTime = 0.8f;
    protected float lastImune;

    // push
    protected Vector3 pushDirection;

    public AudioClip audioClip;

    // receiving dmg
    protected virtual void ReceiveDamage(Damage dmg)
    {
        if (Time.time - lastImune > imuneTime)
        {
            GameManager.instance.player.GetComponent<AudioSource>().PlayOneShot(audioClip);
            lastImune = Time.time;
            hitpoint -= dmg.damageAmount;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

            GameManager.instance.ShowText("-"+dmg.damageAmount.ToString(), 24, Color.red, transform.position, Vector3.zero, 0.5f);

            if (hitpoint <= 0)
            {
                hitpoint = 0;
                Death();
            }
        }
    }
    protected virtual void Death()
    {

    }
}
