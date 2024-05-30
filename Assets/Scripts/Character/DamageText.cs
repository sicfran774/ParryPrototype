using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public float floatRate;

    void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * floatRate;
    }

    public void UpdateText(int damage, Transform origin)
    {
        GetComponent<TMP_Text>().text = "" + damage;
        transform.position = origin.position;
    }
}
