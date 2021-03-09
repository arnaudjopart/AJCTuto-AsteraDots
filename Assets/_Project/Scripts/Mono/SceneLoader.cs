using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string[] m_scenesToLoad;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var sceneName in m_scenesToLoad)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
    }


    public void LoadSceneByName(string _name)
    {
        SceneManager.LoadSceneAsync(_name,LoadSceneMode.Additive);
    }

}
