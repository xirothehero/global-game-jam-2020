using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    string state = "patrolling";
    public bool moveLeft = false;
    public float speedOfEnemy = 5;
    public int enemyHealth;


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

    IEnumerator KnockBack()
    {
        Vector2 thisPos = transform.position;
        while (transform.position.x <= (thisPos.x + 50))
        {
            transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y+0.1f);
            yield return new WaitForSeconds(0.5f);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "wall" && moveLeft)
            moveLeft = false;
        else if (collision.gameObject.tag == "wall")
            moveLeft = true;

        if (collision.gameObject.tag == "player" && !canFollow)
        {
            playerObj = collision.gameObject;
            playerObj.GetComponent<Player>().health -= playerObj.GetComponent<Player>().damageTaken ;
        }

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
            if (enemyHealth <= 0)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Debug.Log("Starting knockback");
                StartCoroutine("KnockBack");
            }
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
