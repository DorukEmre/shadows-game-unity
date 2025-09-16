using UnityEngine;
using UnityEngine.InputSystem;

public class Level2 : AbstractLevelController
{

  protected override void EnableInteraction()
  {
    CheckMouseButtonPressed();

    CheckMouseButtonReleased();

    CheckWin();

    RotateXAndYIfDragging();
  }

  protected override void IsWinConditionMet()
  {
    float x = transform.rotation.eulerAngles.x;
    float y = transform.rotation.eulerAngles.y;
    float z = transform.rotation.eulerAngles.z;

    // Win conditions:
    // (x), (90 || 270), (90 || 270) 
    // 270, (45 || 225), (135 || 315)
    // 270, (135 || 315), (45 || 225)
    // (90 || 270), (0 || 180), (0 || 180)

    if (
        ((Is(y, 90) || Is(y, 270)) && (Is(z, 90) || Is(z, 270)))
        || (Is(x, 270) && (Is(y, 45) || Is(y, 225)) && (Is(z, 135) || Is(z, 315)))
        || (Is(x, 270) && (Is(y, 135) || Is(y, 315)) && (Is(z, 45) || Is(z, 225)))
        || ((Is(x, 90) || Is(x, 270)) && (Is(y, 0) || Is(y, 180)) && (Is(z, 0) || Is(z, 180)))
      )
    {
      lm.TriggerWin();
    }
  }

}
