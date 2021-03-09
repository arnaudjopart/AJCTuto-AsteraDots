using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericGameEvent<T> : ScriptableObject {

    private List<GenericGameEventListener<T>> m_listeners = new List<GenericGameEventListener<T>>();

    public virtual void Raise(T t)
    {
        //Debug.Log("Raise Event - " + name);

        for (int i = m_listeners.Count - 1; i >= 0; i--)
        {
            m_listeners[i].OnEventRaised(t);
        }
    }
    public void RegisterListener(GenericGameEventListener<T> _listener)
    {
        m_listeners.Add(_listener);
    }
    public void UnegisterListener(GenericGameEventListener<T> _listener)
    {
        m_listeners.Remove(_listener);
    }
}
