using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelVictoryMenu : MonoBehaviour
{
  [SerializeField] private AudioClip victoryAudioClip;
  private AudioSource audioSource;

  void Awake()
  {
    audioSource = GetComponent<AudioSource>();
    if (audioSource == null || victoryAudioClip == null)
      Debug.LogError("Audio missing from LevelVictoryMenu");
  }

  public void LoadLevelsMap()
  {
    Debug.Log("Loading LevelsMap Scene");
    SceneManager.LoadScene("LevelsMap");
  }

  public void QuitGame()
  {
    Debug.Log("Quitting Game");
    Application.Quit();
  }


  public void ProcessVictory()
  {
    Debug.Log("You won!");
    // Update level completion status in GameManager
    var gm = GameManager.Instance;
    if (gm != null)
    {
      if (gm.levelStates[gm.currentLevelIndex] != LevelState.Completed)
      {
        gm.levelStates[gm.currentLevelIndex] = LevelState.Completed;
        gm.newlyCompletedIndex = gm.currentLevelIndex;
      }

      if (gm.currentLevelIndex + 1 < gm.levelStates.Length
          && gm.levelStates[gm.currentLevelIndex + 1] == LevelState.Locked)
      {
        gm.levelStates[gm.currentLevelIndex + 1] = LevelState.Unlocked;
        gm.newlyUnlockedIndex = gm.currentLevelIndex + 1;
      }
    }

    // Move camera
    Camera.main.GetComponent<CameraMover>().MoveToWall();

    // Show victory panel with victory message and buttons to main menu or quit
    gameObject.SetActive(true);

    // Flicker light and make ring larger
    GameObject spotLight = GameObject.FindGameObjectWithTag("SpotLight");
    Light spot = spotLight.GetComponent<Light>();
    StartCoroutine(FlickerLight(spot));
    spot.spotAngle = 28f;

    audioSource.PlayOneShot(victoryAudioClip);
  }

  private System.Collections.IEnumerator FlickerLight(Light spot, float duration = 1f)
  {
    float originalIntensity = spot.intensity;
    float timer = 0f;
    while (timer < duration)
    {
      spot.intensity = Random.Range(5f, 22f);
      yield return new WaitForSeconds(Random.Range(0.02f, 0.1f));
      timer += Time.deltaTime;
    }
    spot.intensity = originalIntensity;
  }
}
