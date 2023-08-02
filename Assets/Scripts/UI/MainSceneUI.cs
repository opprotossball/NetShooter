using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainSceneUI : MonoBehaviour
{
    public static MainSceneUI instance;
    public TMP_Text gameCode;
    public TMP_Text text;
    public Image shade;
    public Image bullet;
    public Image bulletBack;
    public Image grenade;
    public Image grenadeBack;
    public Image bulletCooldown;
    public Image grenadeCooldown;
    [SerializeField] private float activeAlpha;
    [SerializeField] private float inactiveAlpha;
    public float cooldown;
    public Weapon activeWeapon;
    public float endScreenTime = 3;
    public float shadeAlpha = 0.63f;

    public void SetActiveWeapon(Weapon weapon)
    {
        activeWeapon = weapon;
    }

    public void ShowCooldowns()
    {
        Image active = null;
        List<Image> inactive = new List<Image>();
        if (activeWeapon == Weapon.RIFLE)
        {
            grenadeCooldown.fillAmount = 1;
            bulletCooldown.fillAmount = cooldown;
        } 
        else if (activeWeapon == Weapon.GRENADE)
        {
            active = grenadeCooldown;
            inactive.Add(bulletCooldown);
        }
        if (active is not null)
        {
            active.fillAmount = cooldown;
        }
        foreach (Image image in inactive)
        {
            image.fillAmount = 1;
        }
    }

    private void SetAlpha(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    public void SetGameCode(string code)
    {
        gameCode.text = code;
    }
    private void Start()
    {
        SetActiveWeapon(Weapon.RIFLE);
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (GameManager.instance.GetGameState() != GameState.ACTIVE)
        {   
            SetAlpha(shade, shadeAlpha);
            text.text = "Waiting for opponent...";
        } 
        else
        {
            SetAlpha(shade, 0);
            text.text = "";
        }
    }

    public IEnumerator ShowEnd(bool victory)
    {
        SetAlpha(shade, shadeAlpha);
        if (victory)
        {
            text.text = "You won!";
        } else
        {
            text.text = "You lost!";
        }
        yield return new WaitForSeconds(endScreenTime);
        SceneManager.LoadScene("Menu");
    }

}
