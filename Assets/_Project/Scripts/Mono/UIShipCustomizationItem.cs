using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Mono;
using UnityEngine;
using _Project.Scripts.ScriptableObjects.Achievements;
using _Project.Scripts.ScriptableObjects.Reward;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIShipCustomizationItem : MonoBehaviour
{
    public Achievement m_achievement;

    public ShipCustomizationEvent m_onClickEvent;
    private Button m_button;
    public GameObject m_checkMark;

    private void Awake()
    {
        m_onClickEvent = new ShipCustomizationEvent();
        m_button = GetComponent<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    

    public void Unlock()
    {
        m_button.interactable = true;
    }

    public void Lock()
    {
        m_button.interactable = false;
    }

    public void OnClick()
    {
        m_onClickEvent.Invoke(m_achievement.m_guid);
    }

    public RewardScriptableObject[] GetReward()
    {
        return m_achievement.m_rewards;
    }

    public void Deselect()
    {
        m_checkMark.SetActive(false);
    }
    
    public void Select()
    {
        m_checkMark.SetActive(true);
    }
}

public class ShipCustomizationEvent : UnityEvent<string>{}