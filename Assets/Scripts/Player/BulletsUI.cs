using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsUI : MonoBehaviour
{
    [SerializeField] private Rifle weapon;
    [SerializeField] private SpriteRenderer[] bullets;
    [SerializeField] private Sprite fullBullet;
    [SerializeField] private Sprite emptyBullet;
    [SerializeField] private int numOfBullets;

    void Update()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i].enabled = i < numOfBullets;
            if (i < weapon.GetLoadedAmmo()) 
            {
                bullets[i].sprite = fullBullet;
            } else {
                bullets[i].sprite = emptyBullet;
            } 

        }
    }
}
