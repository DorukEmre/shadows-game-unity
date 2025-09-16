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
  private int nObjects = 0;

  public static event System.Action OnInteractionAllowed;

  private System.Collections.Generic.List<AbstractLevelController> registeredControllers = new System.Collections.Generic.List<AbstractLevelController>();

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

  public void RegisterLevelController(AbstractLevelController controller)
  {
    if (!registeredControllers.Contains(controller))
    {
      Debug.Log("Registering level controller: " + controller.name);
      registeredControllers.Add(controller);
      nObjects++;
    }

    controller.OnWinConditionMet += OnLevelWinConditionMet;
    controller.OnWinConditionLost += OnLevelWinConditionLost;
  }

  private void OnLevelWinConditionMet(AbstractLevelController controller)
  {
    Debug.Log($"Win condition met for: {controller.name}");

    if (nObjects != registeredControllers.Count)
      Debug.LogWarning("nObjects does not match registeredControllers count!");

    int objWithWinConditionMet = 0;
    foreach (var ctrl in registeredControllers)
    {
      Debug.Log($"Registered controller: {ctrl.name}, WinConditionMet: {ctrl.winConditionMet}");
      if (!ctrl.winConditionMet)
        return;
      else
        objWithWinConditionMet++;
    }

    if (objWithWinConditionMet == nObjects)
      TriggerWin();
  }

  private void OnLevelWinConditionLost(AbstractLevelController controller)
  {
    // Handle win condition lost for a controller
    Debug.Log($"Win condition lost for: {controller.name}");
    // Add logic if needed
    Debug.Log("registeredControllers count: " + registeredControllers.Count);
    foreach (var ctrl in registeredControllers)
    {
      Debug.Log($"Registered controller: {ctrl.name}, WinConditionMet: {ctrl.winConditionMet}");
    }
  }
}
