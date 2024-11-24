using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Image healthBarPng;

    private void Start()
    {
        healthBarPng.fillAmount = 1;
    }

    private void Update()
    {
        healthBarPng.fillAmount = player.currentHealth / 100.0f;
    }

}
