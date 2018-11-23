using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectScroller : MonoBehaviour {

    protected float screenWidth;
    protected float halfWidth;

    Camera currentCamera;

    // Use this for initialization
    void Start()
    {
        foreach (Camera c in Camera.allCameras)
        {
            if (c.CompareTag("BackgroundCamera"))
            {
                this.currentCamera = c;

                this.screenWidth = ScreenUtils.GetCameraRightEdge(this.currentCamera) - ScreenUtils.GetCameraLeftEdge(this.currentCamera);

                break;
            }
        }

        this.halfWidth = this.gameObject.GetComponent<BoxCollider2D>().size.x / 2;
    }

    // Update is called once per frame
    void Update () 
    {
        // if the dirt is no longer on the screen
        if (!checkOnScreen())
        {
            // move the dirt to the other side of the screen
            Vector3 currentPosition = transform.position;
            float newX = GetNewX(currentPosition.x);
            Vector3 newPosition = new Vector3(newX, currentPosition.y, currentPosition.z);
            transform.position = newPosition;

            AddDecoration();
        } 
    }

    protected abstract float GetNewX(float currentX);

    protected abstract void AddDecoration();

    private bool checkOnScreen()
    {
        float rightEdge = transform.position.x + halfWidth;
        // dirt if on screen if right edge of dirt is further right than screen left
        return rightEdge > ScreenUtils.GetCameraLeftEdge(this.currentCamera);
    }

    //private float getCameraLeftEdge()
    //{
    //    float bgScreenZ = -this.currentCamera.transform.position.z;
    //    Vector3 bgLowerLeftCornerScreen = new Vector3(0, 0, bgScreenZ);
    //    Vector3 bgLowerLeftCornerWorld =
    //        this.currentCamera.ScreenToWorldPoint(bgLowerLeftCornerScreen);

    //    return bgLowerLeftCornerWorld.x;
    //}

    //private float getCameraRightEdge()
    //{
    //    float bgScreenZ = -this.currentCamera.transform.position.z;
    //    Vector3 bgUpperRightCornerScreen = new Vector3(
    //        Screen.width, Screen.height, bgScreenZ);
    //    Vector3 bgUpperRightCornerWorld =
    //        this.currentCamera.ScreenToWorldPoint(bgUpperRightCornerScreen);

    //    return bgUpperRightCornerWorld.x;
    //}
}
