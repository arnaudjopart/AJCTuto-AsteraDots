using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class BoolReference {

    public bool m_useConstant = true;
    public bool m_constantValue;
    public BoolVariable m_variable;
   

    public bool Value {
        get { return m_useConstant ? m_constantValue:m_variable.m_value; }
        set { if(!m_useConstant) m_variable.m_value = value; }
    }
}
