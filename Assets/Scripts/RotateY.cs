using UnityEngine;
using UnityEngine.InputSystem;

public class RotateY : MonoBehaviour
{

  public float rotationSpeed = 1f;
  private bool isDragging = false;
  private bool hasWon = false;
  private GameObject victoryMenu;
  private GameObject pauseMenu;

  void Start()
  {
    victoryMenu = GameObject.FindGameObjectWithTag("VictoryMenu");
    if (victoryMenu != null)
      victoryMenu.SetActive(false);

    pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
    if (pauseMenu != null)
      pauseMenu.SetActive(false);
  }

  void Update()
  {
    if (Mouse.current == null || hasWon)
      return;

    // Toggle esc menu visibility on esc key press
    if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
    {
      pauseMenu.SetActive(!pauseMenu.activeSelf);
      if (pauseMenu.activeSelf)
      {
        pauseMenu.GetComponent<LevelPauseMenu>().SlideInBars();
      }
    }

    if (pauseMenu.activeSelf) // Check if escape menu is active (game paused)
      return;

    // On mouse button down, check if the object is under the cursor
    if (Mouse.current.leftButton.wasPressedThisFrame)
    {
      Debug.Log("Mouse Button Pressed");
      Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()); // Create a ray from the camera through the mouse position
      RaycastHit hit; // Variable to store raycast hit information
      if (Physics.Raycast(ray, out hit)) // Perform the raycast
      {
        Debug.Log("RaycastHit hit: " + hit.transform.name);
        if (hit.transform == transform)
        {
          isDragging = true;
        }
      }
    }

    // On mouse button up, stop dragging
    if (Mouse.current.leftButton.wasReleasedThisFrame)
    {
      Debug.Log("Mouse Button Released");
      isDragging = false;
      float finalYRotation = transform.rotation.eulerAngles.y;
      if ((finalYRotation > 0 && finalYRotation < 10)
      || (finalYRotation > 350 && finalYRotation < 360)
      || (finalYRotation > 170 && finalYRotation < 190))
      {
        hasWon = true;
        Victory();
      }
    }

    // Only rotate if dragging this object
    if (isDragging && Mouse.current.leftButton.isPressed)
    {
      float mouseX = Mouse.current.delta.ReadValue().x;
      // Debug.Log("Rotating: " + Mouse.current.delta.ReadValue() + ", mouseX: " + mouseX);
      Debug.Log("Current Y Rotation: " + transform.rotation.eulerAngles.y);
      transform.Rotate(0, mouseX * rotationSpeed, 0);
    }
  }

  void Victory()
  {
    Debug.Log("You won!");
    // Update level completion status in GameManager
    var gm = GameManager.Instance;
    if (gm != null)
    {
      if (gm.levelStates[gm.currentLevelIndex] != LevelState.Completed)
      {
        gm.levelStates[gm.currentLevelIndex] = LevelState.Completed;
        gm.newlyCompletedIndex = gm.currentLevelIndex;
      }

      if (gm.currentLevelIndex + 1 < gm.levelStates.Length
          && gm.levelStates[gm.currentLevelIndex + 1] == LevelState.Locked)
      {
        gm.levelStates[gm.currentLevelIndex + 1] = LevelState.Unlocked;
        gm.newlyUnlockedIndex = gm.currentLevelIndex + 1;
      }
    }

    // Move camera
    Camera.main.GetComponent<CameraMover>().MoveToWall();

    // Show victory panel with victory message and buttons to main menu or quit
    victoryMenu.SetActive(true);

    // Flicker light and make ring larger
    GameObject spotLight = GameObject.FindGameObjectWithTag("SpotLight");
    Light spot = spotLight.GetComponent<Light>();
    StartCoroutine(FlickerLight(spot));
    spot.spotAngle = 28f;

  }

  private System.Collections.IEnumerator FlickerLight(Light spot, float duration = 1f)
  {
    float originalIntensity = spot.intensity;
    float timer = 0f;
    while (timer < duration)
    {
      spot.intensity = Random.Range(5f, 22f);
      yield return new WaitForSeconds(Random.Range(0.02f, 0.1f));
      timer += Time.deltaTime;
    }
    spot.intensity = originalIntensity;
  }
}
