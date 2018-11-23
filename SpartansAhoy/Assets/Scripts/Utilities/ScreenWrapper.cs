using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScreenWrapper {

    /// <summary>
    /// Checks if the specified transform with the specified colliderRadius
    /// is outside the bounds of the screen and, if so, moves it to the other 
    /// side of the screen.
    /// </summary>
    /// <param name="transform">Transform.</param>
    /// <param name="colliderRadius">Collider radius.</param>
    public static void ScreenWrapMainCamera(Transform transform, float colliderRadius)
    {
        // get the current position of the transform
        Vector3 objPosition = transform.position;

        float objMinX = objPosition.x - colliderRadius;
        float objMaxX = objPosition.x + colliderRadius;
        float objMinY = objPosition.y - colliderRadius;
        float objMaxY = objPosition.y + colliderRadius;

        // test if off left side of screen
        if (objMinX < ScreenUtils.MainScreenLeft)
        {
            objPosition.x = ScreenUtils.MainScreenRight + colliderRadius;
        }

        // test if off right side of screen
        if (objMaxX > ScreenUtils.MainScreenRight)
        {
            objPosition.x = ScreenUtils.MainScreenLeft - colliderRadius;
        }

        // test if off bottom of screen
        if (objMinY < ScreenUtils.MainScreenBottom)
        {
            objPosition.y = ScreenUtils.MainScreenTop + colliderRadius;
        }

        // test if off top of screen
        if (objMaxY > ScreenUtils.MainScreenTop)
        {
            objPosition.y = ScreenUtils.MainScreenBottom - colliderRadius;
        }

        // set the new position of the transform
        transform.position = objPosition;
    }

    public static void ScreenWrapBackgroundCamera(Transform transform, float colliderRadius)
    {
        // get the current position of the transform
        Vector3 objPosition = transform.position;

        float objMinX = objPosition.x - colliderRadius;
        float objMaxX = objPosition.x + colliderRadius;
        float objMinY = objPosition.y - colliderRadius;
        float objMaxY = objPosition.y + colliderRadius;

        // test if off left side of screen
        if (objMinX < ScreenUtils.BgScreenLeft)
        {
            objPosition.x = ScreenUtils.BgScreenRight + colliderRadius;
        }

        // test if off right side of screen
        if (objMaxX > ScreenUtils.BgScreenRight)
        {
            objPosition.x = ScreenUtils.BgScreenLeft - colliderRadius;
        }

        // test if off bottom of screen
        if (objMinY < ScreenUtils.BgScreenBottom)
        {
            objPosition.y = ScreenUtils.BgScreenTop + colliderRadius;
        }

        // test if off top of screen
        if (objMaxY > ScreenUtils.BgScreenTop)
        {
            objPosition.y = ScreenUtils.BgScreenBottom - colliderRadius;
        }

        // set the new position of the transform
        transform.position = objPosition;
    }
}
