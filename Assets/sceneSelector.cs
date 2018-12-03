using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneSelector : MonoBehaviour
{

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
     void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.Escape)){
            Application.Quit();
        }
    }
}
