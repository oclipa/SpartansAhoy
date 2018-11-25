using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpartyScroller : ObjectScroller
{
    protected override float GetNewLeftX(float currentX)
    {
        return ScreenUtils.GetCameraLeftEdge(Camera.main);
    }

    protected override float GetNewRightX(float currentX)
    {
        return ScreenUtils.GetCameraRightEdge(Camera.main);
    }

    protected override void UpdateDisplay()
    {
    }

    protected override bool ExitScreenLeft()
    {
        return transform.position.x < ScreenUtils.GetCameraLeftEdge(Camera.main);
    }

    protected override bool ExitScreenRight()
    {
        return transform.position.x > ScreenUtils.GetCameraRightEdge(Camera.main);
    }
}
