using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayGame() {
        SceneManager.LoadScene("Level0");
    }

    public void ExitGame() {
        Application.Quit();
    }
}
