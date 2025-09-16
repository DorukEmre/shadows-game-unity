using UnityEngine;

public class LevelManager1 : AbstractLevelManager
{
  private GameObject levelObject;

  protected override void Start()
  {
    base.Start();

    if (levelObjects.Length > 0)
      levelObject = levelObjects[0];
    else
      Debug.LogError("No level objects assigned in LevelManager.");
  }

  protected override void IsWinConditionMet()
  {
    float yRotation = levelObject.transform.rotation.eulerAngles.y;

    if ((yRotation > 0 && yRotation < 10)
    || (yRotation > 350 && yRotation < 360)
    || (yRotation > 170 && yRotation < 190))
    {
      TriggerWin();
    }
  }
}
