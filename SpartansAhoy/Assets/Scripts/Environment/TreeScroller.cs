using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScroller : ObjectScroller
{
    protected override float GetNewLeftX(float currentX)
    {
        return currentX;
    }

    protected override void UpdateDisplay()
    {
        Destroy(this.gameObject);
    }
}
