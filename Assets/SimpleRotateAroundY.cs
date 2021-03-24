using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotateAroundY : MonoBehaviour
{
    public float m_speed=10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var localUp = transform.InverseTransformVector(Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis( m_speed * Time.deltaTime, localUp);
    }
}
