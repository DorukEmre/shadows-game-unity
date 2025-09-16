using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages level state, victory and pause menus, and interaction permissions.
/// Publishes an event to allow subscribed level objects to enable interaction when appropriate.
/// Singleton pattern that does NOT persist across scenes.
/// </summary>
public abstract class AbstractLevelManager : MonoBehaviour
{
  public static AbstractLevelManager Instance;

  private bool hasWon = false;
  [SerializeField] private GameObject victoryMenu;
  [SerializeField] private GameObject pauseMenu;

  [SerializeField] public GameObject[] levelObjects;

  public static event System.Action OnInteractionAllowed;

  void Awake()
  {
    if (Instance == null)
      Instance = this;
    else
      Destroy(gameObject);
  }

  protected virtual void Start()
  {
    if (victoryMenu == null || pauseMenu == null)
      Debug.LogError("Error loading menus in LevelManager.");

    victoryMenu.SetActive(false);

    pauseMenu.SetActive(false);

    if (levelObjects.Length == 0)
      Debug.LogError("No level objects assigned in LevelManager.");
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

  /// <summary>
  /// Trigger the win condition, show victory menu
  /// </summary>
  protected void TriggerWin()
  {
    hasWon = true;

    victoryMenu.GetComponent<LevelVictoryMenu>().ProcessVictory();
  }

  /// Check for pause menu input (Escape key)
  void CheckPauseMenuInput()
  {
    if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
    {
      pauseMenu.SetActive(!pauseMenu.activeSelf);
      if (pauseMenu.activeSelf)
      {
        pauseMenu.GetComponent<LevelPauseMenu>().SlideInBars();
      }
    }
  }

  public void RegisterObjectController(AbstractObjectController controller)
  {
    controller.OnCheckWinCondition += OnCheckWinCondition;
  }

  private void OnCheckWinCondition(AbstractObjectController controller)
  {
    IsWinConditionMet();
  }

  protected abstract void IsWinConditionMet();

  /// <summary>
  /// Is 'value' angle near 'target' (360 degrees taken into account)
  /// </summary>
  protected bool Is(float value, float target, float tolerance = 10f)
  {
    return Mathf.Abs(Mathf.DeltaAngle(value, target)) < tolerance;
  }
}
