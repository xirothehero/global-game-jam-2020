using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voidout : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag.Equals("Player"))
        {
            other.GetComponent<Player>().TakeDamage(100);
        }
    }
}
