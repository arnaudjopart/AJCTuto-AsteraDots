using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class IntReference {

    public bool m_useConstant = true;
    public int m_constantValue;
    public IntVariable m_variable;

    public int Value
    {
        get { return m_useConstant ? m_constantValue : m_variable.Value; }
        
    }
}
