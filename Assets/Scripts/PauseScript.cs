using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    private InputAction _pauseAction;

    private Canvas _canvas;

    private bool _paused;

    private void Start()
    {
        _pauseAction = InputSystem.actions.FindAction("Pause");

        _canvas = transform.GetChild(0).gameObject.GetComponent<Canvas>();

        _paused = false;
        _canvas.enabled = false;
    }

    private void Update()
    {
        if (_pauseAction.WasPressedThisFrame())
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        _paused = !_paused;

        _canvas.enabled = _paused;
        
        if (_paused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
