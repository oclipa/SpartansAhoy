using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainScroller : ObjectScroller
{
    protected override float GetNewLeftX(float currentX)
    {
        return currentX + this.screenWidth + (this.halfWidth * 2);
    }

    protected override void UpdateDisplay()
    {
    }
}
