using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseFollow : MonoBehaviour
{
    public GameObject point;
    public bool pathReached = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveToMouse());

        //StartCoroutine(coroutineA());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MoveToMouse() {
        Vector3 mousePos = point.transform.position;//Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        yield return null;
        while(true) {
            if (Vector3.Distance(transform.position, mousePos) < 0.01) {
                pathReached = true;
            }
        }
    }

//  yield return new WaitForSeconds(Random.Range(0.0f, 2f));
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
    }
}
