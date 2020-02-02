using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cutscene : MonoBehaviour
{
    Animator anim;
    int scene = 0;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            scene += 1;
            anim.SetInteger("Scene", scene);
            if (scene >= 4) {
                SceneManager.LoadScene(2);
            }
        }
        
    }
}
