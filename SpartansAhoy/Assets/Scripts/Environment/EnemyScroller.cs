using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScroller : ObjectScroller
{
    protected override float GetNewLeftX(float currentX)
    {
        return ScreenUtils.GetCameraLeftEdge(Camera.main);
    }

    protected override void UpdateDisplay()
    {
    }

    protected override bool ExitScreenLeft()
    {
        return transform.position.x < ScreenUtils.GetCameraLeftEdge(Camera.main);
    }
}
