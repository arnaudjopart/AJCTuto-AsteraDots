using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IntGameEventListener : GenericGameEventListener<int>
{
    public IntUnityEvent m_response;
    public IntGameEvent m_event;

    public override void OnEventRaised(int t)
    {
        m_response.Invoke(t);
    }

    protected override void OnDisable()
    {
        m_event.UnegisterListener(this);
    }

    protected override void OnEnable()
    {
        m_event.RegisterListener(this);
    }
    
}
[System.Serializable]
public class IntUnityEvent : UnityEvent<int>
{

}
