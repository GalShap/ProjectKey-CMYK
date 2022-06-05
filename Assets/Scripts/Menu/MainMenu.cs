using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu: MonoBehaviour
{
    private IEnumerator wait( float time)
    {
        yield return new WaitForSeconds(time);
    }


    public void LoadGame()
    {
        StartCoroutine(wait(0.2f));
        SceneManager.LoadScene("Game");
    }

    public void Exit()
    {
        StartCoroutine(wait(0.2f));
        Application.Quit();
    }
    
}
