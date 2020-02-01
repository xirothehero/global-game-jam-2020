using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour {

    [SerializeField] private Transform _spawnPoint;

    public Transform SpawnPoint => _spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag.Equals("Player"))
        {
            GameManager.instance.IncrementSegment();
        }
    }
}
