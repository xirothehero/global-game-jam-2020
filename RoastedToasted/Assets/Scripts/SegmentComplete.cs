using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentComplete : MonoBehaviour
{
    public GameObject[] platforms;
    public GameObject tear;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            foreach(GameObject platform in platforms) {
                platform.GetComponent<Platform>().complete();
                tear.GetComponent<Animator>().SetTrigger("fade");
            }
        }
    }
}
