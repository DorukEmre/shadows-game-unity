using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Changes the color and light state of a level box based on completion status.
/// Loads corresponding scene on click.
/// 
/// levelNumber (assigned by user in the interface) is applied to the TextMeshProUGUI component.
/// Relevant Material is assigned to renderer of the "Body" child (Cube).
/// Light component is toggled on/off based on completion status.
/// Hint text (assigned by user in the interface) is shown/hidden on hover.
/// </summary>
[ExecuteAlways]
public class LevelsMapBox
  : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
  private GameManager gm = GameManager.Instance;

  public int levelNumber;
  [SerializeField] private TextMeshProUGUI levelNumberTextComponent;

  private Renderer levelBoxRenderer;
  [SerializeField] private Material completedMaterial;
  [SerializeField] private Material lockedMaterial;

  [SerializeField] private Light bottomLight;
  [SerializeField] private Light topLight;

  public string hint;
  [SerializeField] private TextMeshPro hintTextComponent;

  private bool isPaused = false;
  private bool isUnlocked = false;

  // for top light drifting
  private Vector3 originalTopLightPosition;
  private Vector2 driftDirection = Vector2.zero;
  private float driftTimer = 0f;
  private const float driftChangeInterval = 0.7f;

  void Start()
  {
    if (gm == null || levelNumberTextComponent == null || completedMaterial == null || lockedMaterial == null || hintTextComponent == null || bottomLight == null || topLight == null)
    {
      Debug.LogError("Error loading LevelsMapBox" +
        (gm == null ? " GameManager" : "") +
        (levelNumberTextComponent == null ? " levelNumberTextComponent" : "") +
        (completedMaterial == null ? " completedMaterial" : "") +
        (lockedMaterial == null ? " lockedMaterial" : "") +
        (hintTextComponent == null ? " hintTextComponent" : "") +
        (bottomLight == null ? " bottomLight" : "") +
        (topLight == null ? " topLight" : "")
      );
      return;
    }

    // Get Renderer component of the "Body" to apply materials
    Transform bodyChild = transform.Find("Body");
    if (bodyChild != null)
    {
      levelBoxRenderer = bodyChild.GetComponent<Renderer>();
    }

    if (levelBoxRenderer == null)
    {
      Debug.LogError("Renderer component not found on the Cube of Level Box");
      return;
    }

    levelNumberTextComponent.text = levelNumber.ToString();

    hintTextComponent.text = hint;
    hintTextComponent.enabled = false;

    originalTopLightPosition = topLight.transform.position;

    AssignState();
  }

  void Update()
  {
    if (LevelsMapManager.Instance != null)
      isPaused = LevelsMapManager.Instance.IsPaused();

    if (isPaused)
    {
      hintTextComponent.enabled = false;
      return;
    }

    if (isUnlocked)
      driftTopLight();
  }

  void OnValidate()
  {
    if (levelNumberTextComponent != null)
      levelNumberTextComponent.text = levelNumber.ToString();

    if (hintTextComponent != null)
      hintTextComponent.text = hint;
  }

  void AssignState()
  {
    // Set the material and light based on level completion status
    if (gm != null && gm.levelStates != null)
    {
      int levelIndex = levelNumber - 1;
      if (levelIndex >= gm.levelStates.Length)
      {
        Debug.LogError("Level index out of bounds: " + levelIndex);
        return;
      }

      if (gm.levelStates[levelIndex] == LevelState.Completed)
      {
        if (levelIndex == gm.newlyCompletedIndex && gm.newlyCompletedIndex >= 0)
        {
          setUnlockedLevel(); // Temporarily set to unlocked for animation
          isUnlocked = false;
        }
        else
          setCompletedLevel();
      }
      else if (gm.levelStates[levelIndex] == LevelState.Unlocked)
      {
        if (levelIndex == gm.newlyUnlockedIndex && gm.newlyUnlockedIndex >= 0)
          setLockedLevel(); // Temporarily set to locked for animation
        else
          setUnlockedLevel();
      }
      else
      {
        setLockedLevel();
      }
    }
    else
    {
      Debug.LogError("GameManager or levelStates array is not properly set up");
    }
  }

  private void setCompletedLevel()
  {
    levelBoxRenderer.material = completedMaterial;
    bottomLight.enabled = true;
    topLight.enabled = true;
    topLight.innerSpotAngle = 22f;
    topLight.spotAngle = 30f;
    topLight.intensity = 5f;

    Vector3 newPosition = topLight.transform.position;
    newPosition.y = 5f;
    topLight.transform.position = newPosition;
  }

  private void setUnlockedLevel()
  {
    levelBoxRenderer.material = lockedMaterial;
    bottomLight.enabled = false;
    topLight.enabled = true;
    topLight.innerSpotAngle = 5f;
    topLight.spotAngle = 23f;
    topLight.intensity = 17f;

    Vector3 newPosition = topLight.transform.position;
    newPosition.y = 3f;
    topLight.transform.position = newPosition;

    isUnlocked = true;
  }

  private void setLockedLevel()
  {
    levelBoxRenderer.material = lockedMaterial;
    bottomLight.enabled = false;
    topLight.enabled = false;
  }

  public System.Collections.IEnumerator AnimateNewlyCompletedLevel()
  {
    gm.newlyCompletedIndex = -1;
    yield return StartCoroutine(NewlyCompletedEffect());
    setCompletedLevel();
  }

  public System.Collections.IEnumerator AnimateNewlyUnlockedLevel()
  {
    gm.newlyUnlockedIndex = -1;
    yield return StartCoroutine(NewlyUnlockedEffect());
    setUnlockedLevel();
  }

  private System.Collections.IEnumerator NewlyCompletedEffect(float duration = 1f)
  {
    Vector3 originalScale = transform.localScale;
    Vector3 targetScale = originalScale * 1.2f;
    float elapsed = 0f;
    while (elapsed < duration)
    {
      // Pause effect if game is paused
      while (isPaused)
        yield return null;

      float t = elapsed / duration;
      transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
      elapsed += Time.deltaTime;
      yield return null;
    }
    transform.localScale = originalScale;

  }

  private System.Collections.IEnumerator NewlyUnlockedEffect(float duration = 1f)
  {
    Vector3 originalScale = transform.localScale;
    Vector3 targetScale = originalScale * 1.2f;
    float elapsed = 0f;
    while (elapsed < duration)
    {
      // Pause effect if game is paused
      while (isPaused)
        yield return null;

      float t = elapsed / duration;
      transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
      elapsed += Time.deltaTime;
      yield return null;
    }
    transform.localScale = originalScale;
  }

  private void driftTopLight(float driftAmount = 0.01f, float maxDrift = 0.25f)
  {
    driftTimer += Time.deltaTime;
    float randomDriftChangeInterval = Random.Range(driftChangeInterval - 0.15f, driftChangeInterval + 0.15f);
    if (driftTimer >= driftChangeInterval || driftDirection == Vector2.zero)
    {
      driftDirection = new Vector2(
        Random.Range(-1f, 1f),
        Random.Range(-1f, 1f)
      ).normalized;
      driftTimer = 0f;
    }

    Vector3 newPosition = topLight.transform.position;
    newPosition.x += driftDirection.x * driftAmount;
    newPosition.z += driftDirection.y * driftAmount;

    newPosition.x = Mathf.Clamp(newPosition.x, originalTopLightPosition.x - maxDrift, originalTopLightPosition.x + maxDrift);
    newPosition.z = Mathf.Clamp(newPosition.z, originalTopLightPosition.z - maxDrift, originalTopLightPosition.z + maxDrift);

    topLight.transform.position = newPosition;
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    if (isPaused)
      return;

    if (hintTextComponent != null)
      hintTextComponent.enabled = true;
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    if (hintTextComponent != null)
      hintTextComponent.enabled = false;
  }

  public void LoadLevel()
  {
    if (isPaused)
      return;

    if (gm.levelStates[levelNumber - 1] != LevelState.Locked)
    {
      string sceneName = "Level " + levelNumber;
      gm.currentLevelIndex = levelNumber - 1;
      SceneManager.LoadScene(sceneName);
    }
  }
}