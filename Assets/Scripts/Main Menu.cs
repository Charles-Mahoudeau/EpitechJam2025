using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame() {
        SceneManager.LoadScene("Game Scene");
    }

    public void ExitGame() {
        Debug.Log("Exiting Game");
        Application.Quit();
    }
}
