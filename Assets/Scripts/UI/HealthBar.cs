using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider bar;

    public void Initialize(int maxHealth)
    {
        bar.maxValue = maxHealth;
        bar.value = maxHealth;
    }

    public void UpdateHealth(int health)
    {
        bar.value = health;
    }
}
