using UnityEngine;

public class LevelPicker : MonoBehaviour
{
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    Debug.Log("Levels completed status:");
    for (int i = 0; i < GameManager.Instance.levelsCompleted.Length; i++)
    {
      Debug.Log("Level " + (i + 1) + ": " + (GameManager.Instance.levelsCompleted[i] ? "Completed" : "Not Completed"));

    }
  }

  // Update is called once per frame
  void Update()
  {

  }
}
