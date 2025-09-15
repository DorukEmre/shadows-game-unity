using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages level state, victory and pause menus, and interaction permissions.
/// Publishes an event to allow subscribed level objects to enable interaction when appropriate.
/// Singleton pattern that does NOT persist across scenes.
/// </summary>
public class LevelManager : MonoBehaviour
{
  public static LevelManager Instance;

  private bool hasWon = false;
  private GameObject victoryMenu;
  private GameObject pauseMenu;


  void Awake()
  {
    if (Instance == null)
      Instance = this;
    else
      Destroy(gameObject);
  }

  void Start()
  {
    victoryMenu = GameObject.FindGameObjectWithTag("VictoryMenu");
    if (victoryMenu != null)
      victoryMenu.SetActive(false);

    pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
    if (pauseMenu != null)
      pauseMenu.SetActive(false);

    if (victoryMenu == null || pauseMenu == null)
      Debug.LogError("Error loading menus.");
  }

  void Update()
  {
    if (Mouse.current == null || hasWon)
      return;

    CheckPauseMenuInput();

    if (pauseMenu.activeSelf) // Return if pause menu active
      return;

    // Allow interaction with subscribed level objects
    OnInteractionAllowed?.Invoke();
  }

  public static event System.Action OnInteractionAllowed;

  public void TriggerWin()
  {
    hasWon = true;

    victoryMenu.GetComponent<LevelVictoryMenu>().ProcessVictory();
  }

  void CheckPauseMenuInput()
  {
    // Toggle pause menu visibility on esc key press
    if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
    {
      pauseMenu.SetActive(!pauseMenu.activeSelf);
      if (pauseMenu.activeSelf)
      {
        pauseMenu.GetComponent<LevelPauseMenu>().SlideInBars();
      }
    }
  }
}
