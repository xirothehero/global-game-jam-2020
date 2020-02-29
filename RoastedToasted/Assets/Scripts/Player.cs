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

    private bool isFalling = true;

    private bool isGrounded = true;


    public Transform groundCheck;
    [Tooltip("Default value for checkRadius is 0.3f")]
    public float checkRadius = 0.3f;
    public LayerMask whatIsGround;

    public float attackCoolDown = 2;
    float orgCoolDown;

    float check;
    float check2;

    bool inEnemy = false;


    [Tooltip("How long should the flash effect be for the player")]
    public float flashEffectTimer = 1f;
    float orgFlashEffectTimer;
    float flashEffectCoolDown = 0.05f;
    bool wasDamaged = false;


    public float temp = 2.55f;
    int life = 3;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
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

        //if (Input.GetKey(KeyCode.LeftShift) && rb.velocity.y == 0)
        //{
        //    speedRate = runSpeed;
        //}
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
            speedRate = walkSpeed;

        if (speedRate != 0 && !source.isPlaying && isGrounded)
            source.PlayOneShot(walk);

        animator.SetFloat("Speed", speedRate);


        // Just a shortcut to get back to the main menu
        if (Input.GetKey(KeyCode.F1))
        {
            SceneManager.LoadScene("Main Menu");
        }




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
            attackCoolDown = orgCoolDown;
            attackBox.SetActive(false);
            animator.SetBool("attacking", false);
        }



        // For flash effect for player when damaged
        if (wasDamaged && flashEffectTimer > 0)
        {
            Debug.Log("Flash Effect playing?");
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






        if (isGrounded)
        {
            animator.SetBool("fallingDown", false);
            if (isFalling)
            {
                source.PlayOneShot(land);
                isFalling = false;
                check2 = -5;
                check = 0;
            }
  
        }


        // While they aren't on the ground, keep checking to see if they
        // are beginning to fall or not
        if (!isGrounded)
        {
            check = transform.position.y;

            if (check2 < check)
            {
                check2 = check;
                isFalling = false;
            }
            else
            {
                isFalling = true;
            }


        }


        // Play falling down animation at the last moment
        if (!isGrounded && isFalling)
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, -Vector2.up, whatIsGround);
            float distance = Mathf.Abs(hit.point.y - transform.position.y);
            if (distance > temp || hit.point.y * -1 > temp - 0.3)
            {
                animator.SetBool("fallingDown", true);
            }

            animator.SetBool("jumping", false);
        }


    }

    void FixedUpdate()
    {

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            animator.SetBool("jumping", true);
            rb.velocity = Vector2.up * jumpForce;
        }




        // Checking until the player is starting to fall
        // to then play a falling animation



        if (Input.GetKey(KeyCode.D))
        {
            rb.position = new Vector2(rb.position.x + walkSpeed * Time.deltaTime, rb.position.y);

            if (isLeft)
            {
                transform.rotation = new Quaternion(0f, 0f, 0f, 0);
                isLeft = false;
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rb.position = new Vector2(rb.position.x - walkSpeed * Time.deltaTime, rb.position.y);

            if (!isLeft)
            {
                isLeft = true;
                transform.rotation = new Quaternion(0f, 180f, 0f, 0);
            }
        }




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
        //life--;
        //if (life <= 0){
        //    SceneManager.LoadScene("LoseScreen");
        //}
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
        {
            TakeDamage(damageTaken);
            inEnemy = true;
        }
 

        //if (collision.gameObject.tag == "gameOver")
        //    SceneManager.LoadScene("LoseScreen");

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
        //if (collision.gameObject.tag == "nextSegment")
        //{
        //    for (int i = 0; i < GameManager.instance.camera.transitionAreas.Length; i++)
        //    {
        //        if (GameManager.instance.camera.transitionAreas[i] == collision.gameObject)
        //        {
        //            if (GameManager.instance.camera.counter == i)
        //            {
        //                GameManager.instance.camera.NextPoint(i + 1, i);
        //            }

        //            else
        //            {
        //                GameManager.instance.camera.NextPoint(i, i);
        //            }
        //        }
        //    }
        //}
        if (collision.gameObject.tag == "hurt")
            TakeDamage(100);

        else if (collision.gameObject.tag == "enemy")
            inEnemy = false;

    }

}
