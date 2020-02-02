using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public bool selected;
    public Sprite completedSprite;
    private Vector3 offset;

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
        Debug.Log("clicked");
        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;

        Vector3 newPos = Camera.main.ScreenToWorldPoint(new Vector2(mouseX, mouseY));
        print(mouseX + " " + mouseY);
        newPos += offset;
        newPos.z = offset.z;

        transform.position = newPos;
    }

    public void complete() {
        gameObject.GetComponent<SpriteRenderer>().sprite = completedSprite;
    }
}
