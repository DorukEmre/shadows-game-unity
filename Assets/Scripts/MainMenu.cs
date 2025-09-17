using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour, IPointerEnterHandler
{
  [SerializeField] private GameObject selectionSpotLight;

  public void OnPointerEnter(PointerEventData eventData)
  {
    if (selectionSpotLight != null)
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

    SceneManager.LoadScene("LevelsMap");
  }

  public void QuitGame()
  {
    Debug.Log("Quitting game...");
    Application.Quit();
  }
}
