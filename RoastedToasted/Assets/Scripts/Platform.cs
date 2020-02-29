using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private Vector3 offset;
    public Sprite completedSprite;
    public bool set = false;
    public GameObject holdingBG;
    private Color color;

    private bool isHolding = false;

    // A bool to check if the object is in the player
    bool isInPlayer;

    [Range(0.1f, 1f)]
    public float opacity = 0.5f;

    private void Start() {
        color = GetComponent<SpriteRenderer>().material.color;
    }
    private void OnMouseDown() {
        if(!set) {
           color.a = opacity;
           GetComponent<SpriteRenderer>().material.color = color;
           offset = transform.position;
           offset -= Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
           isHolding = true;
        }
        else {
            //TODO: does not work feedback
        }

    }
    private void OnMouseDrag() 
    {
        if (!set) {
            GetComponent<BoxCollider2D>().isTrigger = true;  
            float mouseX = Input.mousePosition.x;
            float mouseY = Input.mousePosition.y;

            Color colorBG = holdingBG.GetComponent<SpriteRenderer>().color;
            holdingBG.GetComponent<SpriteRenderer>().color = Color.Lerp(colorBG, new Color(colorBG.r, colorBG.g, colorBG.b, 0.4f), 0.25f);

            Vector3 newPos = Camera.main.ScreenToWorldPoint(new Vector2(mouseX, mouseY));
            newPos += offset;
            newPos.z = offset.z;

            transform.position = newPos;
        }
    }
    private void OnMouseUp() {
        color.a = 1;
        GetComponent<SpriteRenderer>().material.color = color;
        GetComponent<BoxCollider2D>().isTrigger = false;
        
        if (isInPlayer)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 1f);
            isInPlayer = false;
        }
  

        isHolding = false;
    }

    private void Update()
    {

        if (!isHolding && holdingBG.GetComponent<SpriteRenderer>().color.a != 0f)
        {
            Color colorBG = holdingBG.GetComponent<SpriteRenderer>().color;
            holdingBG.GetComponent<SpriteRenderer>().color = Color.Lerp(colorBG, new Color(colorBG.r, colorBG.g, colorBG.b, 0f), 0.25f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "player")
            isInPlayer = true;
    }

    public void complete() {
        set = true;
        gameObject.GetComponent<SpriteRenderer>().sprite = completedSprite;
    }
}
