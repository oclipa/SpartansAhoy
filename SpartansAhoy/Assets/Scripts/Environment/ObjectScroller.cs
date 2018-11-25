using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectScroller : MonoBehaviour {

    protected float screenWidth;
    protected float halfWidth;

    Camera currentCamera;

    // Use this for initialization
    protected virtual void Start()
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
        // if object is no longer on the screen
        if (ExitScreenLeft())
        {
            // move the object to the other side of the screen
            Vector3 currentPosition = transform.position;
            float newX = GetNewLeftX(currentPosition.x);
            if (newX != 0f)
            {
                Vector3 newPosition = new Vector3(newX, currentPosition.y, currentPosition.z);
                transform.position = newPosition;

                UpdateDisplay();
            }
        }
        else if (ExitScreenRight())
        {
            // move the object to the other side of the screen
            Vector3 currentPosition = transform.position;
            float newX = GetNewRightX(currentPosition.x);
            if (newX != 0f)
            {
                Vector3 newPosition = new Vector3(newX, currentPosition.y, currentPosition.z);
                transform.position = newPosition;

                UpdateDisplay();
            }
        }
    }

    protected virtual float GetNewLeftX(float currentX)
    {
        return 0f;
    }

    protected virtual float GetNewRightX(float currentX)
    {
        return 0f;
    }

    protected abstract void UpdateDisplay();

    protected virtual bool ExitScreenLeft()
    {
        float rightEdge = transform.position.x + halfWidth;
        return rightEdge < ScreenUtils.GetCameraLeftEdge(this.currentCamera);
    }

    protected virtual bool ExitScreenRight()
    {
        float leftEdge = transform.position.x - halfWidth;
        return leftEdge > ScreenUtils.GetCameraRightEdge(this.currentCamera);
    }
}
