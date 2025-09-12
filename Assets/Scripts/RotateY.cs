using UnityEngine;
using UnityEngine.InputSystem;

public class RotateY : MonoBehaviour
{

  public float rotationSpeed = 1f;
  private bool isDragging = false;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (Mouse.current == null)
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
        Debug.Log("You won!");
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
