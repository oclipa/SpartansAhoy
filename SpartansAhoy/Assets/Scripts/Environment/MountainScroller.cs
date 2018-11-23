using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainScroller : ObjectScroller
{
    public GameObject prefabTree;

    protected override float GetNewX(float currentX)
    {
        return currentX + this.screenWidth + (this.halfWidth * 2);
    }

    protected override void AddDecoration()
    {
    }
}
