using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [SerializeField] private List<Segment> _segList;
    [SerializeField] private GameObject player1;
    private int _currentSegment;


    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            _currentSegment = 0;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void OnDeath() {
        player1.transform.position = _segList[_currentSegment].SpawnPoint.position;
    }

    public void IncrementSegment() {
        _currentSegment++;
    }
}
