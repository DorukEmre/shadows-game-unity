using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenus : MonoBehaviour
{
  public void LoadLevels()
  {
    Debug.Log("Loading Level Picker Scene");
    // SceneManager.LoadScene("LevelPicker");
  }

  public void QuitGame()
  {
    Debug.Log("Quitting Game");
    Application.Quit();
  }
}
