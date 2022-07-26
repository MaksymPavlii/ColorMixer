using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject loseScreen;
    [SerializeField]
    private GameObject winScreen;
    [SerializeField]
    private TextMeshProUGUI loseText;
    [SerializeField]
    private TextMeshProUGUI winText;

    public void ShowWinScreen(float result)
    {
        winText.text = "Your Score: " + result + "%";
        winScreen.SetActive(true);
    }

    public void ShowLoseScreen(float result)
    {
        loseText.text = "Your Score: " + result + "%";
        loseScreen.SetActive(true);
    }
}
