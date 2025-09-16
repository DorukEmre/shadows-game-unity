using UnityEngine;

public class Level1 : AbstractLevelController
{

  protected override void EnableInteraction()
  {
    CheckMouseButtonPressed();

    CheckMouseButtonReleased();

    CheckWin();

    // Can only rotate around Y axis
    ManipulateIfDragging();
  }


  protected override void IsWinConditionMet()
  {
    bool isNowMet = false;
    float yRotation = transform.rotation.eulerAngles.y;

    if ((yRotation > 0 && yRotation < 10)
    || (yRotation > 350 && yRotation < 360)
    || (yRotation > 170 && yRotation < 190))
    {
      isNowMet = true;
    }

    NotifyWinConditionMet(isNowMet);
  }
}
