using UnityEngine;

public class CameraMover : MonoBehaviour
{
  public void MoveToWall(float duration = 1f)
  {
    Vector3 targetPosition = new Vector3(0f, 1f, -0.25f);
    Quaternion targetRotation = Quaternion.Euler(0f, 180f, 0f);
    StartCoroutine(MoveToTarget(targetPosition, targetRotation, duration));
  }

  private System.Collections.IEnumerator MoveToTarget(Vector3 targetPosition, Quaternion targetRotation, float duration)
  {
    Vector3 startPosition = transform.position;
    Quaternion startRotation = transform.rotation;
    float elapsed = 0f;
    while (elapsed < duration)
    {
      float t = Mathf.SmoothStep(0, 1, elapsed / duration);
      transform.position = Vector3.Lerp(startPosition, targetPosition, t);
      transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
      elapsed += Time.deltaTime;
      yield return null;
    }
    transform.position = targetPosition;
    transform.rotation = targetRotation;
  }
}
