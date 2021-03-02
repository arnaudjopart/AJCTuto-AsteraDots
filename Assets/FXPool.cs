using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXPool : MonoBehaviour
{
    public GameObject m_prefabFX;
    private static GameObject m_prefab;

    // Start is called before the first frame update
    void Start()
    {
        m_prefab = m_prefabFX;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static GameObject GetFx()
    {
        GameObject instance = Instantiate(m_prefab);
        return instance;
    }
}
