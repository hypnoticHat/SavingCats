using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private bool isPaused = true;
    public void RestartLevel()
    {

        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
    public void LoadLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void Start()
    {

        PauseGame();
    }

    private void Update()
    {
        if (isPaused && Input.anyKeyDown)
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {

        Time.timeScale = 0f;
        isPaused = true;

    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;

    } 
}
