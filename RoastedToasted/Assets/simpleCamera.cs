using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleCamera : MonoBehaviour
{
    Vector3 offset;
    public Transform player;
    private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = Vector3.SmoothDamp(transform.position, player.position, ref velocity, 0.3f);
        newPos.y = offset.y;
        newPos.z = offset.z;
        transform.position = newPos;
    }
}
