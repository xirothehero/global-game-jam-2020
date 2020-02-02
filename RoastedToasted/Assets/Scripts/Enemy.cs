using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Need to fix colliders for enemies
public class Enemy : MonoBehaviour
{
    string state = "patrolling";
    public float speedOfEnemy = 5;
    public int enemyHealth = 2;

    public float knockBackForce = 0.05f;


    [Tooltip("Count down for when the enemy loses sight of player to go back to patrol.")]
    public float countDown = 5;

    [Tooltip("The time it takes until the enemy can attack again (only effects attacks if can follow is true)")]
    public float attackCoolDown = 2f;

    [Tooltip("Can follow the player or not")]
    public bool canFollow = true;

    [Tooltip("For a physical knock back effect")]
    public bool canKnockBack = true;

    [Header("For testing the enemy")]
    public bool immortal = false;



    [Header("Don't touch variables")]
    public bool moveLeft = false;


    bool isAttacked = false;
    float returnCount;
    bool lostSight = false;
    GameObject playerObj;

    // org = original
    float orgAttackCoolDown;

    float pushBackTimer = 0.5f;

    float deathTimer = 1f;
    bool dead = false;

    Animator enemyAnimator;

    // When enemy is hit, a flashing effect is played
    // rendering the sprite in and out
    bool flashEffect = false;
    float flashEffectCoolDown = 0.05f;
    void Start()
    {
        returnCount = countDown;
        orgAttackCoolDown = attackCoolDown;
        enemyAnimator = this.gameObject.GetComponent<Animator>();
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

        if (isAttacked && pushBackTimer > 0)
        {
            pushBackTimer -= Time.deltaTime;
            if (playerObj.transform.rotation.y == 0 && canKnockBack)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + knockBackForce, transform.position.y), 3f);
            }
            else if (canKnockBack)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x - knockBackForce, transform.position.y), 3f);
            }

            if (flashEffect)
            {
                flashEffect = false;
            }
            else if (flashEffectCoolDown <= 0)
            {
                flashEffect = true;
                flashEffectCoolDown = 0.05f;
            }
            else
            {
                flashEffectCoolDown -= Time.deltaTime;
            }
            gameObject.GetComponent<SpriteRenderer>().enabled = flashEffect;

        }
        else if (pushBackTimer <= 0)
        {
            pushBackTimer = 0.5f;
            isAttacked = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }

        if (dead)
        {
            enemyAnimator.SetBool("death", true);
            deathTimer -= Time.deltaTime;
            if (deathTimer <= 0)
            {
                deathTimer = 1f;
                dead = false;
                Destroy(gameObject);
            }

        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAttacked)
        {
            ToggleDirection(collision.gameObject);

        }


        if (collision.gameObject.tag == "player" && !canFollow)
        {
            playerObj = collision.gameObject;
            playerObj.GetComponent<Player>().health -= playerObj.GetComponent<Player>().damageTaken ;
        }

        if (collision.gameObject.tag == "kill")
            dead = true;;

    }

    void ToggleDirection(GameObject plyobj)
    {
        if (plyobj.tag == "wall" && moveLeft)
        {
            moveLeft = false;
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }

        else if (plyobj.tag == "wall")
        {
            moveLeft = true;
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
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

            playerObj = collision.gameObject.transform.parent.gameObject;
            isAttacked = true;
            if (enemyHealth <= 0 && !immortal)
                dead = true;;

        }


        if (!isAttacked)
        {
            ToggleDirection(collision.gameObject);
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
