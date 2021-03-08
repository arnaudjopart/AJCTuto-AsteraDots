using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactToInput : MonoBehaviour
{
    public ParticleSystem m_particleSystem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            m_particleSystem.Play();
        }
        else
        {
            m_particleSystem.Stop();
        }
    }
}
