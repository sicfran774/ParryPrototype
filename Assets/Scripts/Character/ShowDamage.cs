using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowDamage : MonoBehaviour
{
    public GameObject damageText;
    public float textLifetime;

    public void Show(int damage)
    {
        //Debug.Log("here");
        GameObject text = Instantiate(damageText, transform);
        text.GetComponent<DamageText>().UpdateText(damage, transform);

        Destroy(text, textLifetime);
    }
}
