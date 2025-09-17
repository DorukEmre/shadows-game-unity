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
public class LevelPickerBox
  : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
  private GameManager gm = GameManager.Instance;

  public int levelNumber;
  [SerializeField] private TextMeshProUGUI levelNumberTextComponent;

  private Renderer levelBoxRenderer;
  [SerializeField] private Material completedMaterial;
  [SerializeField] private Material lockedMaterial;
  [SerializeField] private Material unlockedMaterial;

  private Light boxLight;

  public string hint;
  [SerializeField] private TextMeshPro hintTextComponent;

  private bool isPaused = false;

  void Start()
  {
    // Get Renderer component of the "Body" to apply materials
    Transform bodyChild = transform.Find("Body");
    if (bodyChild != null)
    {
      levelBoxRenderer = bodyChild.GetComponent<Renderer>();
    }

    if (levelBoxRenderer == null)
    {
      Debug.LogError("Renderer component not found on the Cube child of Level Box");
      return;
    }

    // Get Light component
    Transform lightChild = transform.Find("Light");
    if (lightChild != null)
    {
      boxLight = lightChild.GetComponent<Light>();
    }

    if (boxLight == null)
    {
      Debug.LogError("Light component not found on the Point Light child of Level Box");
      return;
    }

    // Set the TextMeshProUGUI text to the level number
    if (levelNumberTextComponent != null)
    {
      levelNumberTextComponent.text = levelNumber.ToString();
    }
    else
    {
      Debug.LogError("levelNumberTextComponent reference not assigned in the Inspector");
    }

    if (hintTextComponent != null)
    {
      hintTextComponent.text = hint;
      hintTextComponent.enabled = false;
    }
    else
    {
      Debug.LogError("hintTextComponent reference not assigned in the Inspector");
    }

    AssignState();
  }

  void Update()
  {
    if (LevelsMapManager.Instance != null)
      isPaused = LevelsMapManager.Instance.IsPaused();

    if (isPaused)
      hintTextComponent.enabled = false;
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
          levelBoxRenderer.material = unlockedMaterial;
          boxLight.enabled = false;
          gm.newlyCompletedIndex = -1;
          StartCoroutine(NewlyCompletedEffect());
        }
        levelBoxRenderer.material = completedMaterial;
        boxLight.enabled = true;
      }
      else if (gm.levelStates[levelIndex] == LevelState.Unlocked)
      {
        if (levelIndex == gm.newlyUnlockedIndex && gm.newlyUnlockedIndex >= 0)
        {
          levelBoxRenderer.material = lockedMaterial;
          boxLight.enabled = false;
          gm.newlyUnlockedIndex = -1;
          StartCoroutine(NewlyUnlockedEffect());
        }
        levelBoxRenderer.material = unlockedMaterial;
        boxLight.enabled = false;
      }
      else
      {
        levelBoxRenderer.material = lockedMaterial;
        boxLight.enabled = false;
      }
    }
    else
    {
      Debug.LogError("GameManager or levelStates array is not properly set up");
    }
  }

  private System.Collections.IEnumerator NewlyCompletedEffect(float duration = 1f)
  {
    Vector3 originalScale = transform.localScale;
    Vector3 targetScale = originalScale * 1.2f;
    float elapsed = 0f;
    while (elapsed < duration)
    {
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
      float t = elapsed / duration;
      transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
      elapsed += Time.deltaTime;
      yield return null;
    }
    transform.localScale = originalScale;
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