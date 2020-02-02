using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [Header("Camera configuration variables")]
    public GameObject[] cameraPoints;
    public GameObject[] transitionAreas;
    public float transitionSpeed = 0.05f;

    [Header("Don't touch these Vars")]
    public int counter = 0;
    public int counterTransition = 0;
    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, cameraPoints[counter].transform.position) >= 0.2f)
        {
            transitionAreas[counterTransition].SetActive(false);
            transform.position = Vector2.MoveTowards(transform.position, cameraPoints[counter].transform.position, transitionSpeed);
            transform.position = new Vector3(transform.position.x, transform.position.y, -2);
        }
        else
        {
            transitionAreas[counterTransition].SetActive(true);
        }

    }

    public void NextPoint(int setCounter, int setOtherCounter)
    {
        counter = setCounter;
        counterTransition = setOtherCounter;
    }
}
