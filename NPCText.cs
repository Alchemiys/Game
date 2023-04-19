using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCText : Collidble
{
    public string message;

    private float cooldown = 4.0f;
    private float lastshown = -4.0f;

    protected override void OnCollide(Collider2D coll)
    {
        if (Time.time - lastshown > cooldown)
        {
            lastshown = Time.time;
            GameManager.instance.ShowText(message, 20, Color.white, transform.position + new Vector3(0,0.16f,0), Vector3.up, cooldown);
        }
    }
}
