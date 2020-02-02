﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSegment : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag.Equals("Player"))
        {
            GameManager.instance.IncrementSegment();
        }
    }
}
