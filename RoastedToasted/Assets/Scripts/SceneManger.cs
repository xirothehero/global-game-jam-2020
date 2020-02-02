using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManger : MonoBehaviour
{
    public void LoadScene (int level) {
        SceneManager.LoadScene(level);
    }

    public void QuitGame () {
        Application.Quit();
    }
}
