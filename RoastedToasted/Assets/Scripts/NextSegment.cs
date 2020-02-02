using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSegment : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        //Make sure player prefab has tag
        if (other.tag.Equals("Player"))
        {
            GameManager.instance.IncrementSegment();
        }
    }
}
