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
  public int levelNumber;
  [SerializeField] private TextMeshProUGUI levelNumberTextComponent;
  private Renderer levelBoxRenderer;
  [SerializeField] private Material completedMaterial;
  [SerializeField] private Material defaultMaterial;

  private Light boxLight;


  public string hint;
  [SerializeField] private TextMeshPro hintTextComponent;

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

    UpdateLevelBox();
  }

  void OnValidate()
  {
    // Runs after a value change in the Inspector
    if (levelNumberTextComponent != null)
    {
      levelNumberTextComponent.text = levelNumber.ToString();
    }

    if (hintTextComponent != null)
    {
      hintTextComponent.text = hint;
    }
  }

  void UpdateLevelBox()
  {
    // Set the material and light based on level completion status
    if (GameManager.Instance != null && GameManager.Instance.levelsCompleted != null)
    {
      int levelIndex = levelNumber - 1;
      if (levelIndex < GameManager.Instance.levelsCompleted.Length && GameManager.Instance.levelsCompleted[levelIndex])
      {
        levelBoxRenderer.material = completedMaterial;
        boxLight.enabled = true;
      }
      else
      {
        levelBoxRenderer.material = defaultMaterial;
        boxLight.enabled = false;
      }
    }
    else
    {
      Debug.LogError("GameManager or levelsCompleted array is not properly set up");
    }
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    if (hintTextComponent != null)
    {
      hintTextComponent.enabled = true;
    }
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    if (hintTextComponent != null)
    {
      hintTextComponent.enabled = false;
    }
  }

  public void LoadLevel()
  {
    string sceneName = "Level " + levelNumber;
    SceneManager.LoadScene(sceneName);
  }
}