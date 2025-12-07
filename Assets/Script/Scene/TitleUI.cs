using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{

    public void GotoHome()
    {
        Debug.Log("ƒz[ƒ€‚ÉˆÚ“®‚µ‚Ü‚·");
        SceneManager.LoadScene("Home");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Home");
        }
    }
}
