using UnityEngine;
using UnityEngine.InputSystem;

public abstract class AbstractLevelController : MonoBehaviour
{
  protected LevelManager lm;
  protected float rotationSpeed = 1f;
  protected bool isDragging = false;

  protected virtual void Start()
  {
    lm = LevelManager.Instance;
    if (lm == null)
      Debug.LogError("LevelManager instance not found.");
  }

  protected void OnEnable()
  {
    LevelManager.OnInteractionAllowed += EnableInteraction;
  }

  protected void OnDisable()
  {
    LevelManager.OnInteractionAllowed -= EnableInteraction;
  }

  protected void CheckMouseButtonPressed()
  {
    // On mouse button down, check if the object is under the cursor
    if (Mouse.current.leftButton.wasPressedThisFrame)
    {
      Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()); // Create a ray from the camera through the mouse position
      // RaycastHit hit; // Variable to store raycast hit information
      if (Physics.Raycast(ray, out RaycastHit hit)) // Perform the raycast
      {
        Debug.Log("Mouse Button Pressed and RaycastHit hit: " + hit.transform.name);
        if (hit.transform == transform)
        {
          isDragging = true;
        }
      }
    }
  }

  protected void CheckMouseButtonReleased()
  {
    // On mouse button up, stop dragging and check win conditions
    if (Mouse.current.leftButton.wasReleasedThisFrame)
    {
      Debug.Log("Mouse Button Released");
      isDragging = false;
    }
  }

  protected void CheckWin()
  {
    if (Mouse.current.leftButton.wasReleasedThisFrame)
    {
      IsWinConditionMet();
    }
  }

  protected void RotateYIfDragging()
  {
    // Only rotate if dragging this object
    if (isDragging && Mouse.current.leftButton.isPressed)
    {
      float mouseX = Mouse.current.delta.ReadValue().x;
      // Debug.Log("Rotating: " + Mouse.current.delta.ReadValue() + ", mouseX: " + mouseX);
      Debug.Log("Current Y Rotation: " + transform.rotation.eulerAngles.y);
      transform.Rotate(0, mouseX * rotationSpeed, 0);
    }
  }

  protected abstract void EnableInteraction();
  protected abstract void IsWinConditionMet();
}