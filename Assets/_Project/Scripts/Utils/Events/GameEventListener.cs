using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour {

    public GameEvent m_event;
    public UnityEvent m_response;

    private void OnEnable()
    {
        m_event.RegisterListener(this);
    }
    private void OnDisable()
    {
        m_event.UnegisterListener(this);
    }

    public void OnEventRaised()
    {
        //print("OnEventRaised");
        m_response.Invoke();
    }
}
