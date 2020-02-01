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
    public float jumpSpeed = 0.3f;
    public int health = 100;
    public int damageTaken = 10;

    Vector2 orgPos;
    Rigidbody2D rb;

    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        runSpeed *= 10;
        jumpSpeed *= 10;
        walkSpeed *= 10;
        orgPos = this.transform.position;
    }

    void Update()
    {
        Vector2 curPos = this.transform.position;
        float speedRate = 0;

        if (Input.GetKey(KeyCode.LeftShift))
            speedRate = runSpeed;
        else
            speedRate = walkSpeed;

        if (Input.GetKey(KeyCode.D))
            this.transform.position = new Vector2(curPos.x + speedRate, curPos.y);

        if (Input.GetKey(KeyCode.A))
            this.transform.position = new Vector2(curPos.x - speedRate, curPos.y);
        
        if (Input.GetKeyDown(KeyCode.W))
            rb.AddForce(new Vector2(0, jumpSpeed * Time.deltaTime));

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
        else if (collision.gameObject.tag == "enemy")
            health -= damageTaken;
        else if (collision.gameObject.tag == "gameOver")
            SceneManager.LoadScene("lose");
        
        if (health <= 0)
            Respawn();
    }
}
