using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Need to make it so players can try to glitch through the wall.
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

    [Header("Sounds")] 
    public AudioSource source;
    public AudioClip walk;
    public AudioClip land;
    public AudioClip attack;

    // Orignial Position
    Vector2 orgPos;
    Rigidbody2D rb;
    int orgHealth;
    bool isMovingLeft = false;
    bool isMovingRight = false;

    bool canMoveLeft = true;
    bool canMoveRight = true;

    bool isLeft = false;

    private bool isFalling = false;

    public float attackCoolDown = 2;
    float orgCoolDown;


    [Tooltip("How long should the flash effect be for the player")]
    public float flashEffectTimer = 1f;
    float orgFlashEffectTimer;
    float flashEffectCoolDown = 0.05f;
    bool wasDamaged = false;

    int life = 3;

    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        orgPos = this.transform.position;
        animator = GetComponent<Animator>();
        orgHealth = health;
        orgCoolDown = attackCoolDown;
        attackCoolDown = 0;
        orgFlashEffectTimer = flashEffectTimer;
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

        if (speedRate != 0 && !source.isPlaying && rb.velocity.y == 0)
            source.PlayOneShot(walk);

        animator.SetFloat("Speed", speedRate);

        if (attackCoolDown > 0)
            attackCoolDown -= Time.deltaTime;

        // Attacking Mechanic
        if (Input.GetKeyDown(KeyCode.Space) && attackCoolDown <= 0 )
        {
            attackCoolDown = orgCoolDown;
            attackBox.SetActive(true);
            animator.SetBool("attacking", true);
            source.PlayOneShot(attack);
        }
        else
        {
            attackBox.SetActive(false);
            animator.SetBool("attacking", false);
        }



        // For flash effect for player
        if (wasDamaged && flashEffectTimer > 0)
        {
            if (flashEffectCoolDown <= 0)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                flashEffectCoolDown = 0.05f;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                attackCoolDown -= Time.deltaTime;
            }

            flashEffectTimer -= Time.deltaTime;
            if (flashEffectTimer <= 0)
            {
                wasDamaged = false;
                flashEffectTimer = orgFlashEffectTimer;
            }
        }










        //if (Input.GetKey(KeyCode.D) && canMoveRight)
        // Move Right
        if (Input.GetKey(KeyCode.D) && canMoveRight)
        {
            isMovingRight = true;
            if (!canMoveLeft)
                canMoveLeft = true;
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
        if (Input.GetKey(KeyCode.A) && canMoveLeft)
        {
            if (!canMoveRight)
            {
                canMoveRight = true;
            }


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
            if (!isFalling)
                isFalling = true;
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
            
            if (isFalling)
            {
                source.PlayOneShot(land);
                isFalling = false;
            }
        }
        //Debug.Log(health);

    }

    public void TakeDamage(int dmg)
    {
        if (health <= 0)
            Respawn();
        else
            health -= dmg;

        wasDamaged = true;
    }

    void CreateObject() {
    
    }

    void Respawn()
    {
        life--;
        if (life <= 0){
            SceneManager.LoadScene("LoseScreen");
        }
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
            SceneManager.LoadScene("LoseScreen");

        if (collision.gameObject.tag == "wall" && isMovingRight)
        {
            if (!canMoveLeft)
                canMoveLeft = true;
            canMoveRight = false;
        }
        else if (collision.gameObject.tag == "wall" && isMovingLeft)
        {
            if (!canMoveRight)
                canMoveRight = true;
            canMoveLeft = false;
        }
            

        if (collision.gameObject.tag == "finishPoint")
        {
            SceneManager.LoadScene("Win Scene");
        }

        if (collision.gameObject.tag == "hurt")
        {
            // Instant kill on spike
            TakeDamage(100);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "nextSegment")
        {
            for (int i = 0; i < GameManager.instance.camera.transitionAreas.Length; i++)
            {
                if (GameManager.instance.camera.transitionAreas[i] == collision.gameObject)
                {
                    if (GameManager.instance.camera.counter == i)
                    {
                        GameManager.instance.camera.NextPoint(i + 1, i);
                    }

                    else
                    {
                        GameManager.instance.camera.NextPoint(i, i);
                    }
                }
            }
        }
    }

    //private void OnCollisionExit(Collision collision)
    //{

    //    if (collision.gameObject.tag == "wall" && !canMoveLeft)
    //        canMoveLeft = true;

    //    else if (collision.gameObject.tag == "wall" && !canMoveRight)
    //        canMoveRight = true;


    //    Debug.Log(canMoveLeft + " Left collision for can move left");
    //    Debug.Log(canMoveRight + " Left collision for can move Right");
    //}
}
