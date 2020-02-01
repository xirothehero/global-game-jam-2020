using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameObject shape;

    [Header("Player stats")]
    public float walkSpeed = 0.1f;
    public float runSpeed = 0.3f;
    public float jumpForce = 5f;
    public int health = 100;

    [Tooltip("")]
    public int damageTaken = 10;


    public Animator animator;

    // Orignial Position
    Vector2 orgPos;
    Rigidbody2D rb;

    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        orgPos = this.transform.position;
        //animator = GetComponent<Animator>();
    }

    void Checkpoint(Vector2 cp)
    {
        orgPos = cp; 
    }

    void Update()
    {
        Vector2 curPos = this.transform.position;
        float speedRate = 0;

        if (Input.GetKey(KeyCode.LeftShift))
            speedRate = runSpeed;
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            speedRate = walkSpeed;


        animator.SetFloat("Speed", speedRate);


        if (Input.GetKey(KeyCode.D))
        {
            this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            this.transform.position = new Vector2(curPos.x + speedRate, curPos.y);
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            this.transform.position = new Vector2(curPos.x - speedRate, curPos.y);
        }

        float check2 = 0;
        float check = rb.velocity.y;

        if (check2 <= check)
            check2 = check;
        else if (check != 0)
        {
            animator.SetBool("jumping", false);
            animator.SetBool("fallingDown", true);
        }

        Debug.Log(rb.velocity.y);

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

    }

    void CreateObject() {
    
    }

    void Respawn()
    {
        health = 100;
        this.transform.position = orgPos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "kill")
            health -= 100;
        
        if (collision.gameObject.tag == "enemy")
            health -= damageTaken;

        if (collision.gameObject.tag == "gameOver")
            SceneManager.LoadScene("lose");
        
        if (health <= 0)
            Respawn();
    }
}
