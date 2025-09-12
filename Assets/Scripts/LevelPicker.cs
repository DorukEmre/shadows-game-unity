using UnityEngine;

public class LevelPicker : MonoBehaviour
{
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    string completedLevels = "Completed Levels: ";
    for (int i = 0; i < GameManager.Instance.levelsCompleted.Length; i++)
    {
      completedLevels += (i + 1) + ": " + (GameManager.Instance.levelsCompleted[i] ? "Yes" : "No") + " | ";
    }
    Debug.Log(completedLevels);

  }

  // Update is called once per frame
  void Update()
  {

  }
}
