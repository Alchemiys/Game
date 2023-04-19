using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        SaveState();
        instance = this;
        SceneManager.sceneLoaded += LoadState;
        DontDestroyOnLoad(gameObject);
    }

    //Resources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;

    // References
    public Player player;
    public Weapon weapon;
    public FloatingTextManager floatingTextManager;
    public RectTransform hitpointBar;
    public Animator deathMenuAnim;

    //Logic
    public int pesos;
    public int experience;

    //floating text
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize,color, position, motion, duration);
    }
    //upgrade weapon
    public bool TryUpgradeWeapon()
    {
        // is weapon max?
        if (weaponPrices.Count <= weapon.weaponLvl)
        {
            return false;
        }
        if (pesos >= weaponPrices[weapon.weaponLvl])
        {
            pesos -= weaponPrices[weapon.weaponLvl];
            weapon.UpgradeWeapon();
            return true;
        }
        return false;
    }

    //Hitpoint bar
    public void OnHitpointChange()
    {
        float ratio = (float)player.hitpoint / (float)player.maxHitpoing;
        hitpointBar.localScale = new Vector3(1, ratio, 1);
    }

    //Death Menu and Respawn
    public void Respawn()
    {
        deathMenuAnim.SetTrigger("Hide");

        string s = "";
        s += "0" + "|";
        s += "0" + "|";
        s += "0" + "|";
        s += "0";

        PlayerPrefs.SetString("SaveState", s);

        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        player.Respawn();
    }

    //Exp system
    public int GetCurrentLvl()
    {
        int r = 0;
        int add = 0;
        while (experience >= add)
        {
            add += xpTable[r];
            r++;

            if (r == xpTable[r])
                return r;
        }
        return r;
    }
    public int GetXpToLvl(int level)
    {
        int r = 0;
        int xp = 0;

        while (r < level)
        {
            xp += xpTable[r];
            r++;
        }
        return xp;
    }
    public void GrantXp(int xp)
    {
        int currLvl = GetCurrentLvl();
        experience += xp;
        if (currLvl < GetCurrentLvl())
        {
            OnLevelUp();
        }
    }
    public void OnLevelUp()
    {
        player.OnLevelUp();
        OnHitpointChange();
    }

    // load and save
    // preferedSkin || pesos || exp || weaponLvl
    public void SaveState()
    {
        string s = "";
        s += "0" + "|";
        s += pesos.ToString() + "|";
        s += experience.ToString() + "|";
        s += weapon.weaponLvl.ToString();

        PlayerPrefs.SetString("SaveState", s);
    }

    public void LoadState(Scene s, LoadSceneMode mode)
    {
        Debug.Log("Load state");

        if (!PlayerPrefs.HasKey("SaveState"))
        {
            return;
        }
        string[] data = PlayerPrefs.GetString("SaveState").Split('|');
        //set skin

        //set pesos
        pesos = int.Parse(data[1]);
        //set exp
        experience = int.Parse(data[2]);
        if (GetCurrentLvl() != 1)
            player.SetLevel(GetCurrentLvl());
        //set weapon
        weapon.SetWeaponLvl(int.Parse(data[3]));

        player.transform.position = GameObject.Find("SpawnPoint").transform.position;

        Debug.Log("Load state");
    }
}
