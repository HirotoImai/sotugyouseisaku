using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeBackUI : MonoBehaviour
{
    public void GoToHome()
    {
        SceneManager.LoadScene("Home");
    }
}
