using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScroller : ObjectScroller
{
    public Sprite[] cloudSprites;

    protected override void Start()
    {
        base.Start();

        UpdateDisplay();
    }


    protected override float GetNewLeftX(float currentX)
    {
        return currentX + this.screenWidth + (this.halfWidth * 2);
    }

    protected override void UpdateDisplay()
    {
        int chanceOfCloud = Random.Range(0, cloudSprites.Length);
        GetComponent<SpriteRenderer>().sprite = cloudSprites[chanceOfCloud];
    }
}
