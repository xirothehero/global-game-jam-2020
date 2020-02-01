using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    string state = "patrolling";
    public bool moveLeft = false;
    public float speedOfEnemy = 5;

    [Tooltip("Count down for when the enemy loses sight of player to go back to patrol.")]
    public float countDown = 5;


    float returnCount;
    bool lostSight = false;

    void Start()
    {
        returnCount = countDown;    
    }

    void Update()
    {
        if (state == "patrolling")
        {
            Vector2 thisPos = transform.position;
            if (moveLeft)
                transform.position = new Vector2(thisPos.x - speedOfEnemy, thisPos.y);
            else
                transform.position = new Vector2(thisPos.x + speedOfEnemy, thisPos.y);
        }

        if (lostSight && countDown > 0)
            countDown -= Time.deltaTime;
        else if (countDown <= 0 || !lostSight)
        {
            countDown = returnCount;
            state = "patrolling";
        }

   
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "wall" && moveLeft)
            moveLeft = false;
        else if (collision.gameObject.tag == "wall")
            moveLeft = true;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "player")
        {
            state = "chasing";
            lostSight = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            lostSight = true;
    }
}
