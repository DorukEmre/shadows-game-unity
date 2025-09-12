using UnityEngine;

public class MainMenu : MonoBehaviour
{
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }
  public void PlayGame(string mode)
  {

    Debug.Log("Game mode selected: " + mode);

    PlayerPrefs.SetString("GameMode", mode);
    UnityEngine.SceneManagement.SceneManager.LoadScene("LevelPicker");
  }

  public void QuitGame()
  {
    Debug.Log("Quitting game...");
    Application.Quit();
  }
}
