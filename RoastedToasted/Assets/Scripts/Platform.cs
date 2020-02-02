using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private Vector3 offset;
    public Sprite completedSprite;
    public bool set = false;
    private Color color;

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
        }
        else {
            //TODO: does not work feedback
        }
    }
    private void OnMouseDrag() 
    {
        if(!set) {
            GetComponent<BoxCollider2D>().enabled = false;  
            float mouseX = Input.mousePosition.x;
            float mouseY = Input.mousePosition.y;

            Vector3 newPos = Camera.main.ScreenToWorldPoint(new Vector2(mouseX, mouseY));
            newPos += offset;
            newPos.z = offset.z;

            transform.position = newPos;
        }
    }
    private void OnMouseUp() {
        color.a = 1;
        GetComponent<SpriteRenderer>().material.color = color;
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public void complete() {
        set = true;
        gameObject.GetComponent<SpriteRenderer>().sprite = completedSprite;
    }
}
