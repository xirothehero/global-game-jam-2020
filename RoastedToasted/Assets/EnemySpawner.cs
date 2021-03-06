﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    float offsetY;
    public float min;
    public float max;
    void Start()
    {
        offsetY = transform.position.y;
        StartCoroutine(SpawnEnemy(enemy));
    }

    // Update is called once per frame
    void Update()
    {
        // Follow the player at original height;
        Vector3 newPos = player.transform.position;
        newPos.y = offsetY;
        transform.position = newPos;
    }

    IEnumerator SpawnEnemy(GameObject enemy) {
        yield return new WaitForSeconds(Random.Range(min,max));
        Instantiate(enemy, transform.position, Quaternion.identity);
    }
        
}
