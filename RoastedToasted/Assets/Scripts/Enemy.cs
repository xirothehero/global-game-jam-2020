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

    public bool canFollow = true;


    float returnCount;
    bool lostSight = false;
    GameObject playerObj;

    public float attackCoolDown = 2f;
    float orgAttackCoolDown;

    void Start()
    {
        returnCount = countDown;
        orgAttackCoolDown = attackCoolDown;
    }

    void Update()
    {
        Vector2 thisPos = transform.position;
        Vector2 posToGo = new Vector2(thisPos.x,thisPos.y);
        if (state == "patrolling")
        {
            if (moveLeft)
                posToGo = new Vector2(thisPos.x - speedOfEnemy, thisPos.y);
            else
                posToGo = new Vector2(thisPos.x + speedOfEnemy, thisPos.y);
        }
        else if (state == "chasing" && Vector2.Distance(posToGo, playerObj.transform.position) > 1.6f)
        {
            posToGo = Vector2.MoveTowards(thisPos,playerObj.transform.position, 0.05f);
            attackCoolDown = 0;
        }
        else
        {
            attackCoolDown -= Time.deltaTime;
            if (attackCoolDown <= 0)
            {
                playerObj.GetComponent<Player>().TakeDamage(playerObj.GetComponent<Player>().damageTaken);
                attackCoolDown = orgAttackCoolDown;
            }
            if (playerObj.GetComponent<Player>().health <= 0)
            {
                state = "patrolling";
                lostSight = false;
            }

        }
        //Debug.Log(state);

        transform.position = new Vector2(posToGo.x, thisPos.y);

        if (lostSight && countDown > 0)
            countDown -= Time.deltaTime;
        else if (countDown <= 0)
        {
            countDown = returnCount;
            state = "patrolling";
            lostSight = false;
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
        if (collision.gameObject.tag == "Player" && canFollow)
        {
            state = "chasing";
            lostSight = false;
            playerObj = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            lostSight = true;
        }

      
    }
}
