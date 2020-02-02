using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseFollow : MonoBehaviour
{
    public bool pathReached = false;
    public float maxSpeed = 10;
    public float smoothTime = 0.1f;

    Vector3 mousePos;

    Vector3 velocity = Vector3.zero;
    Rigidbody2D rb;
    Vector3 prevLoc = Vector3.zero;
    Vector3 currPos;
    Vector3 direction;
    bool right = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    //     StartCoroutine(MoveToMouse());

    //     StartCoroutine(coroutineA());
    }

    // Update is called once per frame
    void Update()
    {
        currPos = transform.position;
        mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        Vector3 newPos = Vector3.SmoothDamp(currPos, mousePos, ref velocity, smoothTime, maxSpeed);
        newPos.z = currPos.z;
        transform.position = newPos;

        
        direction = (transform.position - prevLoc) / Time.deltaTime;
        
        if (direction.x > 0.01 && !right) {
            GetComponent<SpriteRenderer>().flipX = true;
            print("right");
            right = true;

        } else if ( direction.x < -0.01 && right) {
            GetComponent<SpriteRenderer>().flipX = false;
            right = false;
        }
        prevLoc = transform.position;
    }


    // IEnumerator MoveToMouse() {
    //     Vector3 mousePos = point.transform.position;//Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
    //     yield return null;
    //     while(true) {
    //         if (Vector3.Distance(transform.position, mousePos) < 0.01) {
    //             pathReached = true;
    //         }
    //     }
    // }

//  yield return new WaitForSeconds(Random.Range(0.0f, 2f));

    /*
    IEnumerator coroutineA()
    {
        // wait for 1 second
        Debug.Log("coroutineA created");
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(coroutineB());
        Debug.Log("coroutineA running again");
    }

    IEnumerator coroutineB()
    {
        Debug.Log("coroutineB created");
        yield return new WaitForSeconds(2.5f);
        Debug.Log("coroutineB enables coroutineA to run");
    } */
}
