using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NameChanger : MonoBehaviour
{
    public GameObject nameinputfieldobj;
    public TMP_InputField nameInputField;
    public TMP_Text nameText; // インスペクターからアタッチする

    public void OnClickName()
    {
        nameinputfieldobj.SetActive(true);
        Debug.Log("名前の変更をします");
    }
    public void OnclickEnter()
    {
        SetPlayerName();
        Debug.Log("名前を決定します");
    }
    public void SetPlayerName()
    {
        string playerName = nameInputField.text;
        nameText.text = playerName;
        nameinputfieldobj.SetActive(false);
        Debug.Log("名前が" + playerName + "になりました");
    }
}
