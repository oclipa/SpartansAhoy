using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScroller : ObjectScroller
{
    public GameObject prefabTree;

    protected override float GetNewX(float currentX)
    {
        return currentX;
    }

    protected override void AddDecoration()
    {
        Destroy(this.gameObject);
    }
}
