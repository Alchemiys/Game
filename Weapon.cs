using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidble
{
    //damage struct
    public int[] damagepoint = { 1, 2, 3, 4, 5, 6, 7};
    public float[] pushForce = { 2.0f, 2.2f, 2.4f, 2.6f, 2.8f, 2.9f, 3.0f};

    //update
    public int weaponLvl = 0;
    private SpriteRenderer spriteRenderer;

    //swing
    private Animator anim;
    private float cooldown = 0.5f;
    private float lastSwing;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }
    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Space))
        { 
            if (Time.time - lastSwing > cooldown)
            {
                lastSwing = Time.time;
                Swing();
            }
        }
    }
    public void OnAttack()
    {
            if (Time.time - lastSwing > cooldown)
            {
                lastSwing = Time.time;
                Swing();
            }
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Fighter")
        {
            if (coll.name == "Player")
            {
                return;
            }
            //send this to damaged figher
            Damage dmg = new Damage
            {
                damageAmount = damagepoint[weaponLvl],
                origin = transform.position,
                pushForce = pushForce[weaponLvl]
            };
            coll.SendMessage("ReceiveDamage", dmg);

            Debug.Log(coll.name);
        }
    }
    private void Swing()
    {
        anim.SetTrigger("Swing");
    }
    public void UpgradeWeapon()
    {
        weaponLvl++;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLvl];

        //change stats
    }
    public void SetWeaponLvl(int level)
    {
        weaponLvl = level;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLvl];

    }
}
