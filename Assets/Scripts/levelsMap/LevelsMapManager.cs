using UnityEngine;
using UnityEngine.InputSystem;

public class LevelsMapManager : MonoBehaviour
{
  public static LevelsMapManager Instance;
  private GameManager gm = GameManager.Instance;
  [SerializeField] private GameObject pauseMenu;
  [SerializeField] private GameObject levelBoxesContainer;
  [SerializeField] private GameObject allCompletedContainer;
  [SerializeField] private AudioClip completedAudioClip;
  [SerializeField] private AudioClip unlockedAudioClip;
  [SerializeField] private AudioClip fanfareAudioClip;
  private AudioSource audioSource;

  private bool isPaused = false;
  private int tempNewlyCompletedIndex = -1;
  private int tempNewlyUnlockedIndex = -1;

  void Awake()
  {
    if (Instance == null)
      Instance = this;
    else
      Destroy(gameObject);

    audioSource = GetComponent<AudioSource>();
    if (audioSource == null || completedAudioClip == null || unlockedAudioClip == null || fanfareAudioClip == null)
      Debug.LogError("Audio missing from LevelsMapManager");
  }

  void Start()
  {
    if (gm != null && gm.levelStates != null)
    {
      string levelStatus = "Level status: ";
      for (int i = 0; i < gm.levelStates.Length; i++)
      {
        levelStatus += (i + 1) + ": ";
        levelStatus += gm.levelStates[i].ToString() + " | ";
      }
      Debug.Log(levelStatus);
    }
    else
    {
      Debug.LogError("GameManager or levelStates array is not properly set up");
    }

    if (pauseMenu == null || levelBoxesContainer == null || allCompletedContainer == null)
    {
      Debug.LogError("Error loading elements.");
      return;
    }

    pauseMenu.SetActive(false);
    allCompletedContainer.SetActive(false);

    tempNewlyCompletedIndex = gm.newlyCompletedIndex;
    tempNewlyUnlockedIndex = gm.newlyUnlockedIndex;
    gm.newlyCompletedIndex = -1;
    gm.newlyUnlockedIndex = -1;

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
        audioSource.Pause();
        allCompletedContainer.SetActive(false);
      }
      else
      {
        audioSource.UnPause();
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

    // If all levels are completed, play sound
    if (gm != null && gm.levelStates != null && gm.hasCompletedAllLevels == false)
    {
      bool allCompleted = true;
      foreach (var state in gm.levelStates)
      {
        if (state != LevelState.Completed)
        {
          allCompleted = false;
          break;
        }
      }
      if (allCompleted)
      {
        gm.hasCompletedAllLevels = true;
        audioSource.PlayOneShot(fanfareAudioClip);
        allCompletedContainer.SetActive(true);
        yield return new WaitForSeconds(2f);
        allCompletedContainer.SetActive(false);
      }
    }
  }

  System.Collections.IEnumerator AnimateNewlyCompletedLevelBox()
  {
    if (tempNewlyCompletedIndex != -1)
    {
      int index = tempNewlyCompletedIndex;
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
    if (tempNewlyUnlockedIndex != -1)
    {
      int index = tempNewlyUnlockedIndex;
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
