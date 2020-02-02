using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameObject shape;

    [Header("Player stats")]
    public float walkSpeed = 0.05f;
    public float runSpeed = 0.1f;
    public float jumpForce = 5f;
    public int health = 100;

    [Tooltip("")]
    public int damageTaken = 10;

    public GameObject attackBox;

    public Animator animator;

    // Orignial Position
    Vector2 orgPos;
    Rigidbody2D rb;
    int orgHealth;
    bool isFlying = false;
    bool isMovingLeft = false;
    bool isMovingRight = false;

    bool canMoveLeft = true;
    bool canMoveRight = true;

    bool isLeft = false;

    public float attackCoolDown = 2;
    float orgCoolDown;

    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        orgPos = this.transform.position;
        animator = GetComponent<Animator>();
        orgHealth = health;
        orgCoolDown = attackCoolDown;
        attackCoolDown = 0;
    }

    public void Checkpoint(Vector2 cp)
    {
        orgPos = cp;
        health = orgHealth;
    }

    void Update()
    {
        Vector2 curPos = this.transform.position;
        float speedRate = 0;

        if (Input.GetKey(KeyCode.LeftShift) && rb.velocity.y == 0)
        {
            speedRate = runSpeed;
        }
        else if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
            speedRate = walkSpeed;


        animator.SetFloat("Speed", speedRate);

        if (attackCoolDown > 0)
            attackCoolDown -= Time.deltaTime;

        // Attacking Mechanic
        if (Input.GetKeyDown(KeyCode.Space) && attackCoolDown <= 0 )
        {
            Debug.Log("attacking");
            attackCoolDown = orgCoolDown;
            attackBox.SetActive(true);
            animator.SetBool("attacking", true);
        }
        else
        {
            attackBox.SetActive(false);
            animator.SetBool("attacking", false);
        }


        //if (Input.GetKey(KeyCode.D) && canMoveRight)
        // Move Right
        if (Input.GetKey(KeyCode.D))
        {
            isMovingRight = true;
            //this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            this.transform.position = new Vector2(curPos.x + speedRate, curPos.y);
            if (isLeft)
            {
                transform.rotation = new Quaternion(0f, 0f, 0f, 0);
                isLeft = false;
            }

        }
        else
            isMovingRight = false;
        //if (Input.GetKey(KeyCode.A) && canMoveLeft)
        if (Input.GetKey(KeyCode.A))
        {
            isMovingLeft = true;
            if (!isLeft)
            {
                isLeft = true;
                transform.rotation = new Quaternion(0f, 180f, 0f, 0);
            }

            this.transform.position = new Vector2(curPos.x - speedRate, curPos.y);
        }
        else
            isMovingLeft = false;

        float check2 = 0;
        float check = rb.velocity.y;

        if (check2 <= check)
            check2 = check;
        else if (check != 0)
        {
            animator.SetBool("jumping", false);
            animator.SetBool("fallingDown", true);
        }

        //Debug.Log(rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.W) && rb.velocity.y == 0)
        {
            animator.SetBool("jumping", true);
            rb.velocity = Vector2.up * jumpForce;
        }
        else if (rb.velocity.y == 0)
        {
            animator.SetBool("fallingDown", false);
            check = 0;
        }
        //Debug.Log(health);

    }

    public void TakeDamage(int dmg)
    {
        Debug.Log("Taking Damage");
        if (health <= 0)
            Respawn();
        else
            health -= dmg;

    }

    void CreateObject() {
    
    }

    void Respawn()
    {
        health = orgHealth;
        canMoveLeft = true;
        canMoveRight = true;
        this.transform.position = orgPos;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "kill")
            TakeDamage(100);
        
        if (collision.gameObject.tag == "enemy")
            TakeDamage(damageTaken);

        if (collision.gameObject.tag == "gameOver")
            SceneManager.LoadScene("lose");

        if (collision.gameObject.tag == "wall" && isMovingRight)
            canMoveRight = false;

        else if (collision.gameObject.tag == "wall" && isMovingLeft)
            canMoveLeft = false;


    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "wall" && !canMoveLeft)
            canMoveLeft = true;

        if (collision.gameObject.tag == "wall" && !canMoveRight)
            canMoveRight = true;
    }
}
