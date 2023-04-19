using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingFountain : Collidble
{
    public int healingAmount = 1;

    private float healCoolDown = 1.0f;
    private float lastHeal = 1.0f;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name != "Player")
            return;

        if (Time.time - lastHeal > healCoolDown)
        {
            lastHeal = Time.time;
            GameManager.instance.player.Heal(healingAmount);
        }
    }
}
