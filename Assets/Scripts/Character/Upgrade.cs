using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Upgrade : MonoBehaviour
{
    public int damageCost;
    public int damageAdd;
    public int healthCost;
    public int healthAdd;

    [Header("References")]
    public TMP_Text currencyText;
    public TMP_Text damageText;
    public TMP_Text damageCostText;
    public TMP_Text healthText;
    public TMP_Text healthCostText;

    public HealthBar healthBar;

    private Player player;
    private Sword sword;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        sword = GetComponentInChildren<Sword>();

        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            UpdateUI();
        }
    }

    public void UpgradeDamage()
    {
        if(player.currency >= damageCost)
        {
            player.currency -= damageCost;
            damageCost += (int)(damageCost * 1.5f);
            sword.damage += damageAdd;
        }

        UpdateUI();
    }

    public void UpgradeHealth()
    {
        if(player.currency >= healthCost)
        {
            player.currency -= healthCost;
            healthCost += (int)(healthCost * 1.5f);
            player.maxHealth += healthAdd;
            player.health = player.maxHealth;

            healthBar.Initialize(player.maxHealth);
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        currencyText.text = "" + player.currency;

        damageText.text = "" + sword.damage;
        damageCostText.text = "" + damageCost;
        healthText.text = "" + player.health;
        healthCostText.text = "" + healthCost;
    }
}
