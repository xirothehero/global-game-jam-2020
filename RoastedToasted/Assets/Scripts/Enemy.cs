using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Need to fix colliders for enemies
public class Enemy : MonoBehaviour
{
    string state = "patrolling";
    public bool moveLeft = false;
    public float speedOfEnemy = 5;
    public int enemyHealth = 2;

    public float knockBackForce = 0.05f;

    [Header("For testing the enemy")]
    public bool immortal = false;


    [Tooltip("Count down for when the enemy loses sight of player to go back to patrol.")]
    public float countDown = 5;

    public bool canFollow = true;


    bool isAttacked = false;
    float returnCount;
    bool lostSight = false;
    GameObject playerObj;

    public float attackCoolDown = 2f;
    float orgAttackCoolDown;

    float pushBackTimer = 0.5f;
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
        if (!isAttacked)
        {
            if (collision.gameObject.tag == "wall" && moveLeft)
                moveLeft = false;
            else if (collision.gameObject.tag == "wall")
                moveLeft = true;
        }


        if (collision.gameObject.tag == "player" && !canFollow)
        {
            playerObj = collision.gameObject;
            playerObj.GetComponent<Player>().health -= playerObj.GetComponent<Player>().damageTaken ;
        }

        if (collision.gameObject.tag == "kill")
            Destroy(gameObject);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && canFollow)
        {
            state = "chasing";
            lostSight = false;
            playerObj = collision.gameObject;
        }

        if (collision.gameObject.tag == "attack")
        {
            enemyHealth -= 1;

            // Debug.Log("Hit Back");
            //transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y + 0.001f);
            playerObj = collision.gameObject.transform.parent.gameObject;
            //this.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * -(knockBackForce / 10));
            isAttacked = true;
            if (enemyHealth <= 0 && !immortal)
                Destroy(gameObject);

        }
    }

    void FixedUpdate()
    {
        if (isAttacked && pushBackTimer > 0)
        {
            pushBackTimer -= Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + knockBackForce, transform.position.y), 3f);
        }
        else if (pushBackTimer <= 0)
        {
            pushBackTimer = 0.5f;
            isAttacked = false;
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
