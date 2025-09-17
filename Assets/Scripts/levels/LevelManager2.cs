using UnityEngine;

public class LevelManager2 : AbstractLevelManager
{
  private GameObject levelObject;

  protected override void Start()
  {
    base.Start();

    if (levelObjects.Length == 1)
      levelObject = levelObjects[0];
    else
      Debug.LogError("Level objects error in LevelManager.");
  }

  protected override void IsWinConditionMet()
  {
    float x = levelObject.transform.rotation.eulerAngles.x;
    float y = levelObject.transform.rotation.eulerAngles.y;
    float z = levelObject.transform.rotation.eulerAngles.z;

    // Win conditions:
    // (x), (90 || 270), (90 || 270) 
    // 90, (45 || 225), (45 || 225)
    // 90, (135 || 315), (135 || 315)
    // 270, (45 || 225), (135 || 315)
    // 270, (135 || 315), (45 || 225)
    // (90 || 270), (0 || 180), (0 || 180)

    if (
        ((Is(y, 90) || Is(y, 270)) && (Is(z, 90) || Is(z, 270)))
        || (Is(x, 90) && (Is(z, 45) || Is(y, 225)) && (Is(z, 45) || Is(y, 225)))
        || (Is(x, 90) && (Is(z, 135) || Is(z, 315)) && (Is(z, 135) || Is(z, 315)))
        || (Is(x, 270) && (Is(y, 45) || Is(y, 225)) && (Is(z, 135) || Is(z, 315)))
        || (Is(x, 270) && (Is(y, 135) || Is(y, 315)) && (Is(z, 45) || Is(z, 225)))
        || ((Is(x, 90) || Is(x, 270)) && (Is(y, 0) || Is(y, 180)) && (Is(z, 0) || Is(z, 180)))
      )
    {
      TriggerWin();
    }
  }
}
