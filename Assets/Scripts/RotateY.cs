using UnityEngine;
using UnityEngine.InputSystem;

public class RotateY : MonoBehaviour
{

  public float rotationSpeed = 1f;
  private bool isDragging = false;
  private bool hasWon = false;
  private GameObject levelMenu;

  void Start()
  {
    levelMenu = GameObject.FindGameObjectWithTag("LevelMenu");
    if (levelMenu != null)
      levelMenu.SetActive(false);
  }

  void Update()
  {
    if (Mouse.current == null || hasWon)
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
      if ((finalYRotation > 0 && finalYRotation < 5)
      || (finalYRotation > 355 && finalYRotation < 360)
      || (finalYRotation > 175 && finalYRotation < 185))
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
    // Move camera
    Camera.main.GetComponent<CameraMover>().MoveToWall();

    // Show victory panel with victory message and buttons to main menu or quit
    levelMenu.SetActive(true);

    // Flicker light and make ring larger
    GameObject spotLight = GameObject.FindGameObjectWithTag("SpotLight");
    Light spot = spotLight.GetComponent<Light>();
    StartCoroutine(FlickerLight(spot));
    spot.spotAngle = 28f;

    // Update level completion status in GameManager

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
