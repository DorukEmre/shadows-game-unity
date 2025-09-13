using UnityEngine;

public enum LevelState { Locked, Unlocked, Completed }

public class GameManager : MonoBehaviour
{
  public static GameManager Instance;
  public LevelState[] levelStates = new LevelState[10];

  void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }
}
