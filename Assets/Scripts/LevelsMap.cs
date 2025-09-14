using UnityEngine;

public class LevelsMap : MonoBehaviour
{
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    if (GameManager.Instance != null && GameManager.Instance.levelStates != null)
    {
      string levelStatus = "Level status: ";
      for (int i = 0; i < GameManager.Instance.levelStates.Length; i++)
      {
        levelStatus += (i + 1) + ": ";
        levelStatus += GameManager.Instance.levelStates[i].ToString() + " | ";
      }
      Debug.Log(levelStatus);
    }
    else
    {
      Debug.LogError("GameManager or levelStates array is not properly set up");

    }

  }

  // Update is called once per frame
  void Update()
  {

  }
}
