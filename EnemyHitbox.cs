using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : Collidble
{
    // dmg
    public int damage = 1;
    public float pushForce = 5;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag=="Fighter" && coll.name == "Player")
        {
            // damage object will be send to player
            Damage dmg = new Damage
            {
                damageAmount = damage,
                origin = transform.position,
                pushForce = pushForce
            };

            coll.SendMessage("ReceiveDamage", dmg);
        }
    }
}
