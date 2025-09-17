using UnityEngine;

public class LevelManager3 : AbstractLevelManager
{
  private GameObject obj1;
  private GameObject obj2;

  protected override void Start()
  {
    base.Start();

    if (levelObjects.Length == 2)
    {
      obj1 = levelObjects[0];
      obj2 = levelObjects[1];
    }
    else
      Debug.LogError("Level objects error in LevelManager.");
  }

  protected override void IsWinConditionMet()
  {
    // if rotation for obj1 and obj2 (0.00, 0.00, 0.00)
    if (IsRotationValidObj1(obj1) && IsRotationValidObj2(obj2))
    {
      float y1 = obj1.transform.position.y; // 1.05
      float y2 = obj2.transform.position.y; // 1.085 to 1.095

      // Debug.Log("Obj: " + obj1.transform.name + ", pos: " + obj1.transform.position + ", rot: " + obj1.transform.rotation.eulerAngles);
      // Debug.Log("Obj: " + obj2.transform.name + ", pos: " + obj2.transform.position + ", rot: " + obj2.transform.rotation.eulerAngles);

      // if absolute between y1 - y2 is between 0.35 and 0.45
      if (Mathf.Abs(y1 - y2) > 0.01f && Mathf.Abs(y1 - y2) < 0.065f)
        TriggerWin();
      else
        Debug.Log("Y difference not valid: " + Mathf.Abs(y1 - y2) + ", y1: " + y1 + ", y2: " + y2);
    }
    else
      Debug.Log("Rotation not valid. Obj1: " + obj1.transform.rotation.eulerAngles + ": " + IsRotationValidObj1(obj1) + ", Obj2: " + obj2.transform.rotation.eulerAngles + ": " + IsRotationValidObj2(obj2));
  }

  private bool IsRotationValidObj1(GameObject levelObject)
  {
    float x = levelObject.transform.rotation.eulerAngles.x;
    float y = levelObject.transform.rotation.eulerAngles.y;
    float z = levelObject.transform.rotation.eulerAngles.z;

    return Is(x, 0, 8) && Is(y, 0, 8) && Is(z, 0, 9);
  }

  private bool IsRotationValidObj2(GameObject levelObject)
  {
    float x = levelObject.transform.rotation.eulerAngles.x;
    float y = levelObject.transform.rotation.eulerAngles.y;
    float z = levelObject.transform.rotation.eulerAngles.z;

    return Is(x, 0, 8)
      && Is(y, 0, 8)
      && (Is(z, 0, 9) || Is(z, 180, 9));
  }

}
