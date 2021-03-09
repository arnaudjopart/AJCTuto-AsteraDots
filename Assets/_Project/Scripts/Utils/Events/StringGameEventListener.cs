using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StringGameEventListener : GenericGameEventListener<string>
{
    public StringUnityEvent m_response;
    public StringGameEvent m_event;
    protected override void OnDisable()
    {
        m_event.UnegisterListener(this);
    }

    protected override void OnEnable()
    {
        m_event.RegisterListener(this);
    }

    public override void OnEventRaised(string t)
    {
        m_response.Invoke(t);
        //print(t);        
    }   
}

[System.Serializable]
public class StringUnityEvent : UnityEvent<string>
{
}


