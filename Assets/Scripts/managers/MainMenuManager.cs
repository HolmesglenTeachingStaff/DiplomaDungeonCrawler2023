using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
  public void LoadLevel(int levelNum)
    {
        SceneManager.LoadScene(levelNum);
    }
  public void TogglePanel(GameObject activatedObject)
  {
       activatedObject.SetActive(true);
  }
    public void Quit()
    {
        Application.Quit();
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
