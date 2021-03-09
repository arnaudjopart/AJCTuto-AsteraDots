using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class FloatReference {

    public bool m_useConstant = true;
    public float m_constantValue;
    public FloatVariable m_variable;

    public float Value
    {
        get { return m_useConstant ? m_constantValue : m_variable.Value; }
    }
}
