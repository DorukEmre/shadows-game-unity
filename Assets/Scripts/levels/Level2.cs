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

    // (x), (90 || 270), (90 || 270) 
    // 270, (45 || 225), (135 || 315)
    // 270, (135 || 315), (45 || 225)
    // (90 || 270), (0 || 180), (0 || 180)

    bool x90 = (x > 80 && x < 100);
    bool x270 = (x > 260 && x < 280);

    bool y0 = (y > 350 || y < 10);
    bool y45 = (y > 35 && y < 55);
    bool y90 = (y > 80 && y < 100);
    bool y135 = (y > 125 && y < 145);
    bool y180 = (y > 170 && y < 190);
    bool y225 = (y > 215 && y < 235);
    bool y270 = (y > 260 && y < 280);
    bool y315 = (y > 305 && y < 325);

    bool z0 = (z > 350 || z < 10);
    bool z45 = (z > 35 && z < 55);
    bool z90 = (z > 80 && z < 100);
    bool z135 = (z > 125 && z < 145);
    bool z180 = (z > 170 && z < 190);
    bool z225 = (z > 215 && z < 235);
    bool z270 = (z > 260 && z < 280);
    bool z315 = (z > 305 && z < 325);

    if (
        ((y90 || y270) && (z90 || z270))
        || (x270 && (y45 || y225) && (z135 || z315))
        || (x270 && (y135 || y315) && (z45 || z225))
        || ((x90 || x270) && (y0 || y180) && (z0 || z180))
      )
    {
      lm.TriggerWin();
    }
  }
}
