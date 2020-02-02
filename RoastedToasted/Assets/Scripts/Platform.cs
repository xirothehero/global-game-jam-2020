using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public bool selected;
    public Sprite completedSprite;
    private Vector3 offset;
    public bool set = false;
    private Color color;

    private void OnMouseDown() {
        //TODO: check if player is not on platform 
        if(true) {
           selected = true;
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
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public void complete() {
        set = true;
        gameObject.GetComponent<SpriteRenderer>().sprite = completedSprite;
    }
}
