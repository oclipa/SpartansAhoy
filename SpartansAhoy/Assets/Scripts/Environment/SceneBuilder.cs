using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneBuilder
{
    private static float platformSpacing = 2.0f;
    private static float coinHeight = 0.5f;
    private static float enemyHeight = 1.0f;

    private static int platformIndex = 0;

    public static void CreatePlatform(GameObject prefabPlatform, GameObject prefabCoin, GameObject prefabEnemy, GameObject prefabMovingEnemy, Vector3 initialPlatformPosition, int platformCount)
    {
        float platformX = initialPlatformPosition.x;
        float platformY = initialPlatformPosition.y;

        float firstX = platformX;

        for (int i = 0; i < platformCount; i++)
        {
            platformIndex++;

            platformX += platformSpacing;
            Vector3 position = new Vector3(platformX, platformY, 0f);
            GameObject platform = Object.Instantiate(prefabPlatform, position, Quaternion.identity);

            platform.name = "Platform " + platformIndex;

            int chanceOfCoin = Random.Range(0, 10);
            if (chanceOfCoin > 2)
            {
                float coinY = platform.transform.position.y + coinHeight;
                Vector3 coinPosition = new Vector3(platform.transform.position.x, coinY, platform.transform.position.z);
                GameObject coin = Object.Instantiate(prefabCoin, coinPosition, Quaternion.identity);
                coin.transform.SetParent(platform.transform);
            }
        }

        float lastX = platformX;

        int chanceOfEnemy = Random.Range(0, 10);
        if (chanceOfEnemy > 2)
        {
            int chanceOfMovingEnemy = Random.Range(0, 10);
            if (chanceOfMovingEnemy > 5)
            {
                float enemyY = platformY + enemyHeight;
                Vector3 enemyPosition = new Vector3(platformX, enemyY, 0f);
                GameObject enemy = Object.Instantiate(prefabMovingEnemy, enemyPosition, Quaternion.identity);
                foreach(Transform child in enemy.transform)
                {
                    if (child.name.Equals("Waypoint1"))
                    {
                        Vector3 waypoint = new Vector3(firstX + platformSpacing - 0.5f, platformY + enemyHeight, 0f);
                        child.position = waypoint;
                    }
                    else if(child.name.Equals("Waypoint2"))
                    {
                        Vector3 waypoint = new Vector3(lastX + 0.5f, platformY + enemyHeight, 0f);
                        child.position = waypoint;
                    }
                }
            }
            else 
            {
                float enemyY = platformY + enemyHeight;
                Vector3 enemyPosition = new Vector3(platformX, enemyY, 0f);
                GameObject enemy = Object.Instantiate(prefabEnemy, enemyPosition, Quaternion.identity);
            }
        }
    }

    public static void CreateMovingPlatform(GameObject prefabMovingPlatform, GameObject prefabCoin, GameObject prefabEnemy, Vector3 initialPlatformPosition)
    {
        float platformX = initialPlatformPosition.x;
        float platformY = initialPlatformPosition.y;

        float firstX = platformX;

        platformIndex++;

        platformX += platformSpacing;
        Vector3 position = new Vector3(platformX, platformY, 0f);
        GameObject movingPlatform = Object.Instantiate(prefabMovingPlatform, position, Quaternion.identity);

        movingPlatform.name = "Moving Platform " + platformIndex;

        Transform platform = null;
        foreach (Transform child in movingPlatform.transform)
        {
            if (child.gameObject.CompareTag("MovingPlatform"))
            {
                platform = child;
                break;
            }
        }

        // moving platforms always have coins on them
        float coinY = platform.position.y + coinHeight;
        Vector3 coinPosition = new Vector3(platform.position.x + 0.2f, coinY, platform.position.z);
        GameObject coin = Object.Instantiate(prefabCoin, coinPosition, Quaternion.identity);
        coin.transform.SetParent(platform);

        // moving platforms always have enemies on them
        float enemyY = platformY + enemyHeight;
        Vector3 enemyPosition = new Vector3(platform.position.x - 0.2f, enemyY, platform.position.z);
        GameObject enemy = Object.Instantiate(prefabEnemy, enemyPosition, Quaternion.identity);
        enemy.transform.SetParent(platform);
    }
}
