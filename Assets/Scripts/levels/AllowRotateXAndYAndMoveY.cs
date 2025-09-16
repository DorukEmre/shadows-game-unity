using UnityEngine;

public class AllowRotateXAndYAndMoveY : AbstractObjectController
{
  protected override void EnableInteraction()
  {
    base.EnableInteraction();

    // Can rotate on X and Y axis and move along Y axis
    ManipulateIfDragging(true, true);
  }
}
