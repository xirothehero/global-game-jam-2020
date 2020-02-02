using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Vector3[] cameraPoints;
    public float transitionSpeed = 0.3f;
    int counter = 0;
    // Update is called once per frame
    void Update()
    {
        if (transform.position != cameraPoints[counter])
            transform.position = Vector2.MoveTowards(transform.position, cameraPoints[counter], transitionSpeed);
    }

    public void NextPoint()
    {
        counter++;
    }
}
