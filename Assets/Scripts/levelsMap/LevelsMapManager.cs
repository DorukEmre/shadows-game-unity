using UnityEngine;
using UnityEngine.InputSystem;

public class LevelsMapManager : MonoBehaviour
{
  public static LevelsMapManager Instance;
  [SerializeField] private GameObject pauseMenu;
  [SerializeField] private GameObject levelBoxesContainer;
  [SerializeField] private AudioClip completedAudioClip;
  [SerializeField] private AudioClip unlockedAudioClip;
  private AudioSource audioSource;

  private bool isPaused = false;

  void Awake()
  {
    if (Instance == null)
      Instance = this;
    else
      Destroy(gameObject);

    audioSource = GetComponent<AudioSource>();
    if (audioSource == null || completedAudioClip == null || unlockedAudioClip == null)
      Debug.LogError("Audio missing from LevelsMapManager");
  }

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

    if (pauseMenu == null || levelBoxesContainer == null)
      Debug.LogError("Error loading elements.");

    pauseMenu.SetActive(false);

    StartCoroutine(AnimateLevelBoxSequence());
  }

  void Update()
  {
    if (Mouse.current == null)
      return;

    CheckPauseMenuInput();

    if (pauseMenu.activeSelf) // Return if pause menu active
      return;
  }

  /// Check for pause menu input (Escape key)
  void CheckPauseMenuInput()
  {
    if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
    {
      pauseMenu.SetActive(!pauseMenu.activeSelf);
      isPaused = pauseMenu.activeSelf;
      if (pauseMenu.activeSelf)
      {
        pauseMenu.GetComponent<LevelPauseMenu>().SlideInBars();
      }
    }
  }

  public bool IsPaused()
  {
    return isPaused;
  }

  System.Collections.IEnumerator AnimateLevelBoxSequence()
  {
    yield return StartCoroutine(AnimateNewlyCompletedLevelBox());
    yield return StartCoroutine(AnimateNewlyUnlockedLevelBox());
  }

  System.Collections.IEnumerator AnimateNewlyCompletedLevelBox()
  {
    if (GameManager.Instance != null
      && GameManager.Instance.newlyCompletedIndex != -1)
    {
      int index = GameManager.Instance.newlyCompletedIndex;
      LevelsMapBox[] boxes = levelBoxesContainer.GetComponentsInChildren<LevelsMapBox>(true);
      LevelsMapBox completedBox = null;
      foreach (var box in boxes)
      {
        if (box.levelNumber == index + 1)
        {
          completedBox = box;
          break;
        }
      }

      if (completedBox != null)
      {
        audioSource.PlayOneShot(completedAudioClip);
        yield return StartCoroutine(completedBox.AnimateNewlyCompletedLevel());
      }
    }
  }

  System.Collections.IEnumerator AnimateNewlyUnlockedLevelBox()
  {
    if (GameManager.Instance != null
      && GameManager.Instance.newlyUnlockedIndex != -1)
    {
      int index = GameManager.Instance.newlyUnlockedIndex;
      LevelsMapBox[] boxes = levelBoxesContainer.GetComponentsInChildren<LevelsMapBox>(true);
      LevelsMapBox unlockedBox = null;
      foreach (var box in boxes)
      {
        if (box.levelNumber == index + 1)
        {
          unlockedBox = box;
          break;
        }
      }

      if (unlockedBox != null)
      {
        yield return StartCoroutine(unlockedBox.AnimateNewlyUnlockedLevel());
        audioSource.PlayOneShot(unlockedAudioClip);
      }
    }

  }
}
