using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSegment : MonoBehaviour
{
    bool entered = false;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag.Equals("Player"))
        {
            if (!entered)
            {
                GameManager.instance.IncrementSegment();
                entered = true;
            }
        }
    }
}
