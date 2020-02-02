using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentComplete : MonoBehaviour
{
    public GameObject[] platforms;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            foreach(GameObject platform in platforms) {
                platform.GetComponent<Platform>().complete();
            }
        }
    }
}
