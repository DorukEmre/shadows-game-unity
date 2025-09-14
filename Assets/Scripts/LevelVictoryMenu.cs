using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelVictoryMenu : MonoBehaviour
{
  public void LoadLevelsMap()
  {
    Debug.Log("Loading LevelsMap Scene");
    SceneManager.LoadScene("LevelsMap");
  }

  public void QuitGame()
  {
    Debug.Log("Quitting Game");
    Application.Quit();
  }
}
