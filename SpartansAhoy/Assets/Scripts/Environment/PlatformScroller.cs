using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScroller : ObjectScroller
{
    public GameObject prefabPlatform;
    public GameObject prefebMovingPlatform;
    public GameObject prefebEnemy;
    public GameObject prefebMovingEnemy;
    public GameObject prefebCoin;

    private GameObject platformParent;

    private bool hasContactOnLeft;
    private bool hasContactOnRight;

    protected override void Start()
    {
        base.Start();

        GetComponent<BoxCollider2D>();

        if (transform.parent != null && transform.parent.gameObject.CompareTag("MovingPlatform"))
            platformParent = transform.parent.gameObject;
    }

    protected override float GetNewLeftX(float currentX)
    {
        return currentX;
    }

    protected override void UpdateDisplay()
    {
        // if this platforms forms the end of a sequence of platforms, create a new platform when it is destroyed
        if (!hasContactOnRight)
            createNewPlatform(transform.position.y);

        if (platformParent != null)
        {
            //Debug.Log("Destroy moving platform");
            Destroy(this.platformParent);
        }
        else
            Destroy(this.gameObject);
    }

    private void createNewPlatform(float platformY)
    {
        // platform must be created off screen right 
        float platformX = ScreenUtils.GetCameraRightEdge(Camera.main);

        // randomly picked a platform length from 1-3
        int platformCount = Random.Range(1, 4);

        int chanceOfMovingPlatform = Random.Range(0, 10);
        if (chanceOfMovingPlatform > 5)
            SceneBuilder.CreateMovingPlatform(prefebMovingPlatform, prefebCoin, prefebEnemy, new Vector3(platformX, platformY, 0f));
        else
            SceneBuilder.CreatePlatform(prefabPlatform, prefebCoin, prefebEnemy, prefebMovingEnemy, new Vector3(platformX, platformY, 0f), platformCount);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider is CircleCollider2D && collision.gameObject.CompareTag("Platform"))
        {
            ContactPoint2D[] contactPoints = new ContactPoint2D[1];
            collision.GetContacts(contactPoints);

            if (contactPoints[0].point.x > this.gameObject.transform.position.x)
            {
                hasContactOnRight = true;
            }
        }
    }
}
