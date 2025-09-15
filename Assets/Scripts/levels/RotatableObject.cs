using UnityEngine;

public class RotatableObject : MonoBehaviour
{
  private Vector3 originalRotation;
  private Vector3 originalPosition;

  void Start()
  {
    originalPosition = transform.position;
    originalRotation = transform.eulerAngles;
  }

  public void ResetPosition()
  {
    transform.position = originalPosition;
    transform.eulerAngles = originalRotation;
  }
}
