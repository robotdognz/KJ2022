using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLogic : MonoBehaviour
{
    public void StartGame()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void TitleScreen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void TitleScreenDelayed()
    {
        StartCoroutine(TitleScreenEnumerator());
    }

    private IEnumerator TitleScreenEnumerator()
    {
        yield return new WaitForSeconds(0.5f);
        TitleScreen();
    }


    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        Destroy(gameObject);
    }


}
