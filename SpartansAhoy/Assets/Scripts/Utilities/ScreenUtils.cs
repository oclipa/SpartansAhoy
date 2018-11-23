using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides screen utilities
/// </summary>
public static class ScreenUtils
{
	#region Fields

	// cached for efficient boundary checking
    static float mainScreenLeft;
    static float mainScreenRight;
    static float mainScreenTop;
    static float mainScreenBottom;

    static float bgScreenLeft;
    static float bgScreenRight;
    static float bgScreenTop;
    static float bgScreenBottom;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the left edge of the screen in world coordinates
    /// </summary>
    /// <value>left edge of the screen</value>
    public static float MainScreenLeft
    {
		get { return mainScreenLeft; }
	}

	/// <summary>
	/// Gets the right edge of the screen in world coordinates
	/// </summary>
	/// <value>right edge of the screen</value>
	public static float MainScreenRight
    {
		get { return mainScreenRight; }
	}

	/// <summary>
	/// Gets the top edge of the screen in world coordinates
	/// </summary>
	/// <value>top edge of the screen</value>
	public static float MainScreenTop
    {
		get { return mainScreenTop; }
	}

	/// <summary>
	/// Gets the bottom edge of the screen in world coordinates
	/// </summary>
	/// <value>bottom edge of the screen</value>
	public static float MainScreenBottom
    {
		get { return mainScreenBottom; }
	}


    /// <summary>
    /// Gets the left edge of the screen in world coordinates
    /// </summary>
    /// <value>left edge of the screen</value>
    public static float BgScreenLeft
    {
        get { return bgScreenLeft; }
    }

    /// <summary>
    /// Gets the right edge of the screen in world coordinates
    /// </summary>
    /// <value>right edge of the screen</value>
    public static float BgScreenRight
    {
        get { return bgScreenRight; }
    }

    /// <summary>
    /// Gets the top edge of the screen in world coordinates
    /// </summary>
    /// <value>top edge of the screen</value>
    public static float BgScreenTop
    {
        get { return bgScreenTop; }
    }

    /// <summary>
    /// Gets the bottom edge of the screen in world coordinates
    /// </summary>
    /// <value>bottom edge of the screen</value>
    public static float BgScreenBottom
    {
        get { return bgScreenBottom; }
    }

    public static float GetCameraLeftEdge(Camera currentCamera)
    {
        float bgScreenZ = -currentCamera.transform.position.z;
        Vector3 bgLowerLeftCornerScreen = new Vector3(0, 0, bgScreenZ);
        Vector3 bgLowerLeftCornerWorld =
            currentCamera.ScreenToWorldPoint(bgLowerLeftCornerScreen);

        return bgLowerLeftCornerWorld.x;
    }

    public static float GetCameraRightEdge(Camera currentCamera)
    {
        float bgScreenZ = -currentCamera.transform.position.z;
        Vector3 bgUpperRightCornerScreen = new Vector3(
            Screen.width, Screen.height, bgScreenZ);
        Vector3 bgUpperRightCornerWorld =
            currentCamera.ScreenToWorldPoint(bgUpperRightCornerScreen);

        return bgUpperRightCornerWorld.x;
    }


    #endregion

    #region Methods

    /// <summary>
    /// Initializes the screen utilities
    /// </summary>
    public static void Initialize()
    {
		// save screen edges in world coordinates
		float mainScreenZ = -Camera.main.transform.position.z;
		Vector3 mainLowerLeftCornerScreen = new Vector3(0, 0, mainScreenZ);
		Vector3 mainUpperRightCornerScreen = new Vector3(
			Screen.width, Screen.height, mainScreenZ);
		Vector3 mainLowerLeftCornerWorld = 
			Camera.main.ScreenToWorldPoint(mainLowerLeftCornerScreen);
		Vector3 mainUpperRightCornerWorld = 
			Camera.main.ScreenToWorldPoint(mainUpperRightCornerScreen);
		mainScreenLeft = mainLowerLeftCornerWorld.x;
		mainScreenRight = mainUpperRightCornerWorld.x;
		mainScreenTop = mainUpperRightCornerWorld.y;
		mainScreenBottom = mainLowerLeftCornerWorld.y;

        foreach (Camera c in Camera.allCameras)
        {
            if (c.CompareTag("BackgroundCamera"))
            {
                float bgScreenZ = -c.transform.position.z;
                Vector3 bgLowerLeftCornerScreen = new Vector3(0, 0, bgScreenZ);
                Vector3 bgUpperRightCornerScreen = new Vector3(
                    Screen.width, Screen.height, bgScreenZ);
                Vector3 bgLowerLeftCornerWorld =
                    c.ScreenToWorldPoint(bgLowerLeftCornerScreen);
                Vector3 bgUpperRightCornerWorld =
                    c.ScreenToWorldPoint(bgUpperRightCornerScreen);
                bgScreenLeft = bgLowerLeftCornerWorld.x;
                bgScreenRight = bgUpperRightCornerWorld.x;
                bgScreenTop = bgUpperRightCornerWorld.y;
                bgScreenBottom = bgLowerLeftCornerWorld.y;

                break;
            }
        }
    }

    #endregion
}
