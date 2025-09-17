using UnityEngine;

public enum LevelState { Locked, Unlocked, Completed }

public class GameManager : MonoBehaviour
{
  public static GameManager Instance;
  public int currentLevelIndex = -1;
  public int newlyCompletedIndex = -1;
  public int newlyUnlockedIndex = -1;

  [HideInInspector]
  public LevelState[] levelStates;

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

  void Start()
  {
    Debug.Log("GameManager started, levelStates length: " + (levelStates != null ? levelStates.Length.ToString() : "null"));
    if (levelStates == null || levelStates.Length == 0)
    {
      Debug.Log("Initializing levelStates array with default values.");
      levelStates = new LevelState[10];
      levelStates[0] = LevelState.Unlocked;
      for (int i = 1; i < levelStates.Length; i++)
      {
        levelStates[i] = LevelState.Locked;
      }
      // levelStates = new LevelState[10] { LevelState.Completed, LevelState.Unlocked, LevelState.Locked, LevelState.Completed, LevelState.Unlocked, LevelState.Locked, LevelState.Completed, LevelState.Unlocked, LevelState.Locked, LevelState.Locked };
    }
  }
}
