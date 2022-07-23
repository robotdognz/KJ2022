using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    // pause menu prefab field
    [SerializeField] private GameObject pauseMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            // instantiate the pause menu
            GameObject theMenu = Instantiate(pauseMenu) as GameObject;
            theMenu.GetComponent<MenuLogic>().PauseGame();
        }
    }

    public void Test()
    {
        Debug.Log("Test");
    }

    public void TestAgain()
    {
        Debug.Log("Test again");
    }
}
