using UnityEngine;
using UnityEngine.InputSystem;

public class LevelsMapManager : MonoBehaviour
{
  public static LevelsMapManager Instance;
  [SerializeField] private GameObject pauseMenu;

  private bool isPaused = false;

  void Awake()
  {
    if (Instance == null)
      Instance = this;
    else
      Destroy(gameObject);
  }

  void Start()
  {
    if (GameManager.Instance != null && GameManager.Instance.levelStates != null)
    {
      string levelStatus = "Level status: ";
      for (int i = 0; i < GameManager.Instance.levelStates.Length; i++)
      {
        levelStatus += (i + 1) + ": ";
        levelStatus += GameManager.Instance.levelStates[i].ToString() + " | ";
      }
      Debug.Log(levelStatus);
    }
    else
    {
      Debug.LogError("GameManager or levelStates array is not properly set up");
    }

    if (pauseMenu == null)
      Debug.LogError("Error loading pause menu.");

    pauseMenu.SetActive(false);
  }

  void Update()
  {
    if (Mouse.current == null)
      return;

    CheckPauseMenuInput();

    if (pauseMenu.activeSelf) // Return if pause menu active
      return;
  }

  /// Check for pause menu input (Escape key)
  void CheckPauseMenuInput()
  {
    if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
    {
      pauseMenu.SetActive(!pauseMenu.activeSelf);
      isPaused = pauseMenu.activeSelf;
      if (pauseMenu.activeSelf)
      {
        pauseMenu.GetComponent<LevelPauseMenu>().SlideInBars();
      }
    }
  }

  public bool IsPaused()
  {
    return isPaused;
  }
}
