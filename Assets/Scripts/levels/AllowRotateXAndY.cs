using UnityEngine;

public class AllowRotateXAndY : AbstractObjectController
{
  protected override void EnableInteraction()
  {
    base.EnableInteraction();

    // Can rotate on X and Y axis
    ManipulateIfDragging(true, false);
  }
}
