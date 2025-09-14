using UnityEngine;

public enum LevelState { Locked, Unlocked, Completed }

public class GameManager : MonoBehaviour
{
  public static GameManager Instance;
  public LevelState[] levelStates = new LevelState[10];
  public int currentLevelIndex = -1;
  public int newlyCompletedIndex = -1;
  public int newlyUnlockedIndex = -1;

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
