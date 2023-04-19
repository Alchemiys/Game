using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{
    private SpriteRenderer spriteRenderer;
    public static Player instance;
    public bool isAlive = true;

    public FloatingJoystick joystick;

    protected override void Start()
    {
        if (Player.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        if (!isAlive)
            return;
        base.ReceiveDamage(dmg);
        GameManager.instance.OnHitpointChange();
    }
    protected override void Death()
    {
        isAlive = false;
        GameManager.instance.deathMenuAnim.SetTrigger("Show");
    }
    public void Heal(int healingAmount)
    {
        if (hitpoint == maxHitpoing)
            return;

        hitpoint += healingAmount;
        if (hitpoint > maxHitpoing)
            hitpoint = maxHitpoing;
        GameManager.instance.ShowText("+" + healingAmount.ToString() + "hp",
                25, Color.green, transform.position, Vector3.up * 30, 1.0f);
        GameManager.instance.OnHitpointChange();
    }
    public void Respawn()
    {
        Heal(maxHitpoing);
        isAlive = true;
        lastImune = Time.time;
        pushDirection = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (isAlive)
        {
            float x = joystick.Horizontal;//Input.GetAxisRaw("Horizontal");
            float y = joystick.Vertical;//Input.GetAxisRaw("Vertical");

            UpdateMotor(new Vector3(x, y, 0));
        }
    }
    public void SwapSprite(int skinId)
    {
        spriteRenderer.sprite = GameManager.instance.playerSprites[skinId];
    }
    public void OnLevelUp()
    {
        maxHitpoing = maxHitpoing + 5;
        hitpoint = maxHitpoing;
    }
    public void SetLevel(int level)
    {
        for (int i =0; i < level; i++)
        {
            OnLevelUp();
        }
    }
}
