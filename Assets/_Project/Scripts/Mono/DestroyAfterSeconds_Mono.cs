using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds_Mono : MonoBehaviour
{
    public float m_liveTime=2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_liveTime -= Time.deltaTime;
        if (m_liveTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
