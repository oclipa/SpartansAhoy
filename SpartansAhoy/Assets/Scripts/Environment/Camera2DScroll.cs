using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2DScroll : MonoBehaviour {

    public float deltaX = 1f;

    bool isMoving;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 currentPosition = transform.position;

        // always move at speed to the right
        float targetX = currentPosition.x + (deltaX * Time.deltaTime);

        Vector3 targetPosition = new Vector3(targetX, currentPosition.y, currentPosition.z);

        transform.position = targetPosition;
    }
}
