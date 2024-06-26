using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_Text popupText;

    public void BigRedEvent()
    {
        ShowPopupTextForSeconds("New Sword Art Unlocked: Double Swing\nGo to Campfire to Equip", 5f);
    }

    public void ShowPopupTextForSeconds(string message, float time)
    {
        StartCoroutine(TextPopup(message, time));
    }

    IEnumerator TextPopup(string message, float time)
    {
        popupText.alpha = 255f;
        popupText.text = message;
        yield return new WaitForSeconds(time);
        popupText.CrossFadeAlpha(0.0f, 2f, false);
    }
}
