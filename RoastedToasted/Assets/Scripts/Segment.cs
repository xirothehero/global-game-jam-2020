using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour {

    [SerializeField] private Transform _spawnPoint;
    public Transform SpawnPoint => _spawnPoint;
}
