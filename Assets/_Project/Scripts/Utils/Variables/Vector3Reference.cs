using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Vector3Reference {

    public bool m_useConstant = true;
    public Vector3 m_constantValue;
    public Vector3Variable m_variable;

    public Vector3 Value
    {
        get { return m_useConstant ? m_constantValue : m_variable.Value; }

    }
    
}
