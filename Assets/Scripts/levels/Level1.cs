using UnityEngine;
using UnityEngine.InputSystem;

public class Level1 : MonoBehaviour
{
  private LevelManager lm;
  public float rotationSpeed = 1f;
  private bool isDragging = false;

  void Start()
  {
    lm = LevelManager.Instance;
    if (lm == null)
      Debug.LogError("LevelManager instance not found.");
  }

  void OnEnable() { LevelManager.OnInteractionAllowed += EnableInteraction; }
  void OnDisable() { LevelManager.OnInteractionAllowed -= EnableInteraction; }

  void EnableInteraction()
  {

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
        lm.WinLevel();
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
}
