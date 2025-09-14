using UnityEngine;
using UnityEngine.EventSystems;


public class MainMenu : MonoBehaviour, IPointerEnterHandler
{
  public GameObject selectionSpotLight;

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

    if (mode == "test")
    {
      GameManager.Instance.levelStates = new LevelState[10] { LevelState.Unlocked, LevelState.Unlocked, LevelState.Unlocked, LevelState.Unlocked, LevelState.Unlocked, LevelState.Unlocked, LevelState.Unlocked, LevelState.Unlocked, LevelState.Unlocked, LevelState.Unlocked };
    }
    else if (mode == "normal")
    {
      GameManager.Instance.levelStates = new LevelState[10] { LevelState.Unlocked, LevelState.Locked, LevelState.Locked, LevelState.Locked, LevelState.Locked, LevelState.Locked, LevelState.Locked, LevelState.Locked, LevelState.Locked, LevelState.Locked };
    }
    else // continue
    {
      GameManager.Instance.levelStates = new LevelState[10] { LevelState.Completed, LevelState.Unlocked, LevelState.Locked, LevelState.Completed, LevelState.Unlocked, LevelState.Locked, LevelState.Completed, LevelState.Unlocked, LevelState.Locked, LevelState.Locked };
    }
    UnityEngine.SceneManagement.SceneManager.LoadScene("LevelsMap");
  }

  public void QuitGame()
  {
    Debug.Log("Quitting game...");
    Application.Quit();
  }
}
