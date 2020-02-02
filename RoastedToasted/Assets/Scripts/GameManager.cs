using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [SerializeField] private List<Segment> _segList;
    [SerializeField] private GameObject player1;
    public CameraScript camera;
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
    
    public void IncrementSegment() {
        _currentSegment++;
        if (_currentSegment < _segList.Count)
            player1.GetComponent<Player>().Checkpoint(_segList[_currentSegment].SpawnPoint.position);
        //else SceneManager.LoadScene("win");
    }
}
