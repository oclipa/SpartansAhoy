using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtScroller : ObjectScroller
{
    public GameObject prefabTree;

    protected override float GetNewX(float currentX)
    {
        return currentX + this.screenWidth + (this.halfWidth * 2);
    }

    protected override void AddDecoration()
    {
        int chanceOfTree = Random.Range(0, 10);

        if (chanceOfTree > 6)
        {
            Vector3 dirtPosition = transform.position;
            Instantiate(prefabTree, new Vector3(dirtPosition.x, dirtPosition.y + 1, dirtPosition.z), Quaternion.identity);
        }
    }
}
