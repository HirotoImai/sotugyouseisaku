using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using TMPro;
public class ResultUI : MonoBehaviour
{

    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text resultText;
    // Start is called before the first frame update
    private void Awake()
    {
        panel.SetActive(false);
    }
    public void ShowResult(bool playerWin)
    {
        panel.SetActive(true);
        resultText.text = playerWin ? "YOU WIN" : "YOU LOSE";
    }
}
