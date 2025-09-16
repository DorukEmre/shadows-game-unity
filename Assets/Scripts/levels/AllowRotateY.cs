using UnityEngine;

public class AllowRotateY : AbstractObjectController
{
  protected override void EnableInteraction()
  {
    base.EnableInteraction();

    // Can only rotate around Y axis
    ManipulateIfDragging(false, false);
  }
}
