using UnityEngine;

public class LevelPicker : MonoBehaviour
{
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    if (GameManager.Instance != null && GameManager.Instance.levelsCompleted != null)
    {
      string completedLevels = "Completed Levels: ";
      for (int i = 0; i < GameManager.Instance.levelsCompleted.Length; i++)
      {
        completedLevels += (i + 1) + ": " + (GameManager.Instance.levelsCompleted[i] ? "Yes" : "No") + " | ";
      }
      Debug.Log(completedLevels);
    }
    else
    {
      Debug.LogError("GameManager or levelsCompleted array is not properly set up");

    }

  }

  // Update is called once per frame
  void Update()
  {

  }
}
