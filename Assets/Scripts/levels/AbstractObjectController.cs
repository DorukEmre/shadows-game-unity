using UnityEngine;
using UnityEngine.InputSystem;

public abstract class AbstractObjectController : MonoBehaviour
{
  protected AbstractLevelManager lm;
  protected float rotationSpeed = 1f;
  protected bool isDragging = false;

  public bool winConditionMet = false;

  [SerializeField] protected float minY = 1f;
  [SerializeField] protected float maxY = 1f;

  public event System.Action<AbstractObjectController> OnCheckWinCondition;
  // public event System.Action<AbstractObjectController> OnWinConditionMet;
  // public event System.Action<AbstractObjectController> OnWinConditionLost;

  void Start()
  {
    lm = AbstractLevelManager.Instance;
    if (lm != null)
      lm.RegisterObjectController(this);
    else
      Debug.LogError("LevelManager instance not found.");
  }

  protected void OnEnable()
  {
    AbstractLevelManager.OnInteractionAllowed += EnableInteraction;
  }

  protected void OnDisable()
  {
    AbstractLevelManager.OnInteractionAllowed -= EnableInteraction;
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
        Debug.Log("Mouse Button Pressed and RaycastHit hit: " + hit.transform.name + ". I am: " + transform.name);
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
    if (isDragging && Mouse.current.leftButton.wasReleasedThisFrame)
    {
      Debug.Log("Mouse Button Released");
      isDragging = false;

      Debug.Log("Obj: " + transform.name + ", pos: " + transform.position + ", rot: " + transform.rotation.eulerAngles);

    }
  }

  /// <summary>
  /// Rotate on X and Y axis and move along Y axis if allowed
  /// By default, only rotate around Y axis. Boolean parameters enable other movements.
  /// </summary>
  protected void ManipulateIfDragging(bool canRotateX = false, bool canMoveY = false)
  {
    if (isDragging && Mouse.current.leftButton.isPressed)
    {
      // Rotate around X axis when Left Ctrl is held
      if (canRotateX && Keyboard.current != null && Keyboard.current.leftCtrlKey.isPressed)
      {
        float mouseY = Mouse.current.delta.ReadValue().y;
        transform.Rotate(Vector3.right, -mouseY * rotationSpeed, Space.World);
      }
      // Move up and down when Left Shift is held
      else if (canMoveY && Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed)
      {
        float mouseY = Mouse.current.delta.ReadValue().y;
        float newY = Mathf.Clamp(transform.position.y + mouseY * rotationSpeed * 0.001f, minY, maxY);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
      }
      // Rotate around Y axis
      else
      {
        float mouseX = Mouse.current.delta.ReadValue().x;
        transform.Rotate(Vector3.up, mouseX * rotationSpeed, Space.World);
      }
    }
  }

  protected void CheckWin()
  {
    if (Mouse.current.leftButton.wasReleasedThisFrame)
    {
      OnCheckWinCondition?.Invoke(this);
    }
  }

  // protected void NotifyWinConditionMet(bool isNowMet)
  // {
  //   Debug.Log("NotifyWinConditionMet: " + isNowMet + " (was " + winConditionMet + ")");
  //   if (isNowMet && !winConditionMet)
  //   {
  //     winConditionMet = true;
  //     OnWinConditionMet?.Invoke(this);
  //   }
  //   else if (!isNowMet && winConditionMet)
  //   {
  //     winConditionMet = false;
  //     OnWinConditionLost?.Invoke(this);
  //   }
  // }

  protected virtual void EnableInteraction()
  {
    CheckMouseButtonPressed();

    CheckMouseButtonReleased();

    CheckWin();
  }

}