using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    public ButtonClickEvent m_clickEvent;

    private void Awake()
    {
        m_clickEvent = new ButtonClickEvent();
    }

    public void OnClick()
    {
        m_clickEvent.Invoke();
    }
}

public class ButtonClickEvent : UnityEvent{}