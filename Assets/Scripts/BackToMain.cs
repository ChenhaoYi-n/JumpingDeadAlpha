using UnityEngine;
using UnityEngine.SceneManagement;
public class BackToMainMenu : MonoBehaviour
{
    // This function will be triggered by the button click
    public void GoToMainPage()
    {
        SceneManager.LoadScene("StartScene");
    }
}
