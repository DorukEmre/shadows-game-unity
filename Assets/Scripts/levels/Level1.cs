using UnityEngine;
using UnityEngine.InputSystem;

public class Level1 : AbstractLevelController
{

  protected override void EnableInteraction()
  {
    CheckMouseButtonPressed();

    CheckMouseButtonReleased();

    CheckWin();

    RotateYIfDragging();
  }


  protected override void IsWinConditionMet()
  {
    float yRotation = transform.rotation.eulerAngles.y;
    if ((yRotation > 0 && yRotation < 10)
    || (yRotation > 350 && yRotation < 360)
    || (yRotation > 170 && yRotation < 190))
    {
      lm.TriggerWin();
    }
  }
}
