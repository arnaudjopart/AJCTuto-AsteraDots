using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Game Events/Simple Game Event")]
public class GameEvent : ScriptableObject {

    private List<GameEventListener> m_listeners = new List<GameEventListener>();
    
    public void Raise()
    {
        
        for(int i = m_listeners.Count -1; i >= 0; i--)
        {
            //Debug.Log("GameEvent -"+name+" - Raise");
            m_listeners[i].OnEventRaised();
        }
    }
    public void RegisterListener(GameEventListener _listener)
    {
        //Debug.Log("Add GameEventListener");
        m_listeners.Add(_listener);
    }
    public void UnegisterListener(GameEventListener _listener)
    {
        //Debug.Log("UnegisterListener GameEventListener");
        m_listeners.Remove(_listener);
    }
}
