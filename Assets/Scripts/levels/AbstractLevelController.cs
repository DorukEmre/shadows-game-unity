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

      Debug.Log(transform.rotation.eulerAngles);

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
    if (isDragging && Mouse.current.leftButton.isPressed)
    {
      float mouseX = Mouse.current.delta.ReadValue().x;
      transform.Rotate(Vector3.up, mouseX * rotationSpeed, Space.World);
    }
  }

  protected void RotateXAndYIfDragging()
  {
    if (isDragging && Mouse.current.leftButton.isPressed)
    {
      if (Keyboard.current != null && Keyboard.current.leftCtrlKey.isPressed)
      {
        float mouseY = Mouse.current.delta.ReadValue().y;
        transform.Rotate(Vector3.right, -mouseY * rotationSpeed, Space.World);
      }
      else
      {
        float mouseX = Mouse.current.delta.ReadValue().x;
        transform.Rotate(Vector3.up, mouseX * rotationSpeed, Space.World);
      }
    }
  }

  /// <summary>
  /// Is 'value' angle near 'target' (360 degrees taken into account)
  /// </summary>
  protected bool Is(float value, float target, float tolerance = 10f)
  {
    return Mathf.Abs(Mathf.DeltaAngle(value, target)) < tolerance;
  }

  protected abstract void EnableInteraction();
  protected abstract void IsWinConditionMet();

}