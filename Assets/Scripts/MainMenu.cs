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

    // PlayerPrefs.SetString("GameMode", mode);
    if (mode == "test")
    {
      GameManager.Instance.levelsCompleted = new bool[10] { true, true, true, true, true, true, true, true, true, true };
    }
    else
    {
      GameManager.Instance.levelsCompleted = new bool[10] { false, false, false, false, false, false, false, false, false, false };
    }
    UnityEngine.SceneManagement.SceneManager.LoadScene("LevelPicker");
  }

  public void QuitGame()
  {
    Debug.Log("Quitting game...");
    Application.Quit();
  }
}
