using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{

    public GameEvent m_startEvent;

    public void OnClick()
    {
        m_startEvent.Raise();
    }
}
