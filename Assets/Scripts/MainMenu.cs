using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class MainMenu : MonoBehaviour, IPointerEnterHandler
{
  [SerializeField] private GameObject selectionSpotLight;
  [SerializeField] private TextMeshProUGUI continueButtonText;
  private bool firstTimePlaying;

  void Start()
  {
    if (GameManager.Instance == null || selectionSpotLight == null || continueButtonText == null)
    {
      Debug.LogError("Loading error.");
      return;
    }

    firstTimePlaying = GameManager.Instance.firstTimePlaying;

    if (firstTimePlaying)
    {
      continueButtonText.color = new Color(91f / 255f, 91f / 255f, 91f / 255f, 1f);
    }
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    if (eventData.pointerEnter.name == "ContinueButton_Text" && firstTimePlaying)
    {
      return;
    }
    else if (selectionSpotLight != null)
    {
      // Move the spotlight to the y position of the hovered button
      Vector3 newPosition = selectionSpotLight.transform.position;
      newPosition.y = eventData.pointerEnter.transform.position.y;
      selectionSpotLight.transform.position = newPosition;
    }
  }

  public void PlayGame(string mode)
  {
    Debug.Log("Game mode selected: " + mode);

    if (firstTimePlaying && mode == "continue")
      return;

    LevelState[] levelStates = GameManager.Instance.levelStates;

    if (mode == "test")
    {
      for (int i = 0; i < levelStates.Length; i++)
      {
        levelStates[i] = LevelState.Unlocked;
      }
    }
    else if (mode == "normal")
    {
      levelStates[0] = LevelState.Unlocked;
      for (int i = 1; i < levelStates.Length; i++)
      {
        levelStates[i] = LevelState.Locked;
      }
    }

    if (firstTimePlaying)
      GameManager.Instance.firstTimePlaying = false;

    SceneManager.LoadScene("LevelsMap");
  }

  public void QuitGame()
  {
    Debug.Log("Quitting game...");
    Application.Quit();
  }
}
