using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI[] countdownTexts;
    public void BeginCountdown()
    {
        StartCoroutine(BeginCountdownCoroutine());
    }
    private IEnumerator BeginCountdownCoroutine()
    {
        SetCountdownTexts("3");
        yield return new WaitForSeconds(1f);
        SetCountdownTexts("2");
        yield return new WaitForSeconds(1f);
        SetCountdownTexts("1");
        yield return new WaitForSeconds(1f);
        SetCountdownTexts("GO");
        yield return new WaitForSeconds(1f);
        SetCountdownTexts("");
    }
    private void SetCountdownTexts(string newText)
    {
        foreach (var countdownText in countdownTexts)
        {
            countdownText.text = newText;
        }
    }
}
