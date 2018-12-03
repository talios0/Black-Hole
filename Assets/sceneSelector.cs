using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneSelector : MonoBehaviour
{
    public int sceneID;
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
