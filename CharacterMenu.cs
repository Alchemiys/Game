using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    //Text fields
    public Text levelText, hitpointText, pesosText, upgradeText, xpText;

    //Logic
    private int currentCharacterSelection = 0;
    public Image characterSelectionSprite;
    public Image weaponSprite;
    public RectTransform xpBar;

    public static CharacterMenu instance;

    public AudioClip audioClip;

    public void Start()
    {
        if (CharacterMenu.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    //Character Selection 
    public void OnArrowClick(bool right)
    {
        GameManager.instance.player.GetComponent<AudioSource>().PlayOneShot(audioClip);
        if (right)
        {
            currentCharacterSelection++;
            if (currentCharacterSelection == GameManager.instance.playerSprites.Count)
            {
                currentCharacterSelection = 0;
            }
            OnSelectionChanged();
        }
        else
        {
            currentCharacterSelection--;
            if (currentCharacterSelection < 0 )
            {
                currentCharacterSelection = GameManager.instance.playerSprites.Count;
            }
            OnSelectionChanged();
        }
    }
    private void OnSelectionChanged()
    {
        characterSelectionSprite.sprite = GameManager.instance.playerSprites[currentCharacterSelection];
        GameManager.instance.player.SwapSprite(currentCharacterSelection);
    }

    // weapon
    public void OnUpgradeClick()
    {
        GameManager.instance.player.GetComponent<AudioSource>().PlayOneShot(audioClip);
        if (GameManager.instance.TryUpgradeWeapon())
        {
            UpdateMenu();
        }
    }

    //character info
    public void UpdateMenu()
    {
        //eqip
        weaponSprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.weapon.weaponLvl];
        if (GameManager.instance.weapon.weaponLvl == GameManager.instance.weaponPrices.Count) {
            upgradeText.text = "MAX";
        }
        else
        {
            upgradeText.text = GameManager.instance.weaponPrices[GameManager.instance.weapon.weaponLvl].ToString();
        }

        //meta
        hitpointText.text = GameManager.instance.player.hitpoint.ToString();
        pesosText.text = GameManager.instance.pesos.ToString();
        levelText.text = GameManager.instance.GetCurrentLvl().ToString();

        //xp
        int currLvl = GameManager.instance.GetCurrentLvl();
        if (currLvl == GameManager.instance.xpTable.Count)
        {
            xpText.text = GameManager.instance.experience.ToString() + " total xp points";
            xpBar.localScale = Vector3.one;
        }
        else
        {
            int prevLvlXp = GameManager.instance.GetXpToLvl(currLvl - 1);
            int currLevelXp = GameManager.instance.GetXpToLvl(currLvl );

            int diff = currLevelXp - prevLvlXp;
            int currXPIntoLvl = GameManager.instance.experience - prevLvlXp;

            float completionRatio = (float)currXPIntoLvl / (float)diff;
            xpBar.localScale = new Vector3(completionRatio, 1, 1);
            xpText.text = currXPIntoLvl.ToString() + " / " +  diff;
        }
    }

    //exit game
    public void OnExit()
    {
        GameManager.instance.player.GetComponent<AudioSource>().PlayOneShot(audioClip);
        Application.Quit();
    }
}
    
