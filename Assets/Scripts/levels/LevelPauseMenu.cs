using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPauseMenu : MonoBehaviour
{
  [SerializeField] private GameObject topBar;
  [SerializeField] private GameObject bottomBar;

  private RectTransform topBarRect;
  private Vector2 startPosTopBar;
  private Vector2 targetPosTopBar;

  private RectTransform bottomBarRect;
  private Vector2 startPosBottomBar;
  private Vector2 targetPosBottomBar;

  public void SlideInBars()
  {
    // Slide in animation for top and bottom bars

    if (topBarRect == null || bottomBarRect == null)
    {
      GetBarPositions();
    }

    topBarRect.anchoredPosition = startPosTopBar;
    StartCoroutine(SlideInAnimation(topBarRect, startPosTopBar, targetPosTopBar, 0.5f));

    bottomBarRect.anchoredPosition = startPosBottomBar;
    StartCoroutine(SlideInAnimation(bottomBarRect, startPosBottomBar, targetPosBottomBar, 0.5f));
  }

  void GetBarPositions()
  {
    topBarRect = topBar.GetComponent<RectTransform>();
    targetPosTopBar = topBarRect.anchoredPosition;
    startPosTopBar = new Vector2(targetPosTopBar.x, -targetPosTopBar.y);

    bottomBarRect = bottomBar.GetComponent<RectTransform>();
    targetPosBottomBar = bottomBarRect.anchoredPosition;
    startPosBottomBar = new Vector2(targetPosBottomBar.x, -targetPosBottomBar.y);
  }

  private System.Collections.IEnumerator SlideInAnimation(RectTransform barRect, Vector2 startPos, Vector2 targetPos, float duration)
  {
    float elapsed = 0f;
    while (elapsed < duration)
    {
      float t = Mathf.SmoothStep(0, 1, elapsed / duration);
      barRect.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
      elapsed += Time.deltaTime;
      yield return null;
    }
    barRect.anchoredPosition = targetPos;
  }

  public void ResumeGame()
  {
    Debug.Log("Resuming Game");
    gameObject.SetActive(false);
  }

  public void LoadLevelsMap()
  {
    Debug.Log("Loading LevelsMap Scene");
    SceneManager.LoadScene("LevelsMap");
  }

  public void ResetRotatableObjects()
  {
    Debug.Log("Resetting RotatableObjects");

    AbstractLevelManager.Instance.ResetRotatableObjects();

    gameObject.SetActive(false);
  }

  public void ReturnMainMenu()
  {
    Debug.Log("Returning to Main Menu");
    SceneManager.LoadScene("MainMenu");
  }

  public void QuitGame()
  {
    Debug.Log("Quitting Game");
    Application.Quit();
  }
}
