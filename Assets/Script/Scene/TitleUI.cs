using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    public void GotoHome()
    {
        SceneManager.LoadScene("Home");
    }
}
