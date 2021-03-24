using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Mono;
using _Project.Scripts.ScriptableObjects.Achievements;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;

public class ShipCustomizationManager : MonoBehaviour
{
    public UIShipCustomizationItem[] m_shipCustomizationItems;
    public UIShipCustomizationItem[] m_weaponCustomizationItems;

    private string m_itemCurrentlySelected;
    private AchievementGuids m_data;
    public static OnCustomisationSelection m_onCustomChoice;

    public Material m_defaultMaterial;
    private void Awake()
    {
        m_onCustomChoice = new OnCustomisationSelection();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        UpdateCustomizationItems(m_data);
        
        foreach (var VARIABLE in m_shipCustomizationItems)
        {
            VARIABLE.Deselect();
            VARIABLE.m_onClickEvent.AddListener(OnShipCustomizationClick);
        }
        
        foreach (var VARIABLE in m_weaponCustomizationItems)
        {
            VARIABLE.Deselect();
            VARIABLE.m_onClickEvent.AddListener(OnWeaponCustomizationClick);
        }

        LoadPreviousSelection();
    }

    private void OnWeaponCustomizationClick(string arg0)
    {
        
    }

    private void LoadPreviousSelection()
    {
        if (string.IsNullOrEmpty(m_itemCurrentlySelected)) return;
        foreach (var VARIABLE in m_shipCustomizationItems)
        {
            if (VARIABLE.m_achievement.m_guid != m_itemCurrentlySelected) continue;
            VARIABLE.Select();
            return;
        }
    }

    private void OnShipCustomizationClick(string arg0)
    {
        foreach (var VARIABLE in m_shipCustomizationItems)
        {
            VARIABLE.Deselect();
        }

        if (m_itemCurrentlySelected != arg0)
        {
            m_itemCurrentlySelected = arg0;
            foreach (var VARIABLE in m_shipCustomizationItems)
            {
                if (VARIABLE.m_achievement.m_guid == arg0)
                {
                    VARIABLE.Select();
                    m_onCustomChoice.Invoke(VARIABLE.m_achievement.m_rewards[0].m_material);
                }
            }
        }
        else
        {
            m_itemCurrentlySelected = "";
            m_onCustomChoice.Invoke(m_defaultMaterial);
        }
    }

    public static void RegisterToCustomEvent(UnityAction<Material> _action)
    {
        m_onCustomChoice.AddListener(_action);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        var json = DataSaveLoadUtils.LoadAchievementData();
        if (string.IsNullOrEmpty(json)) return;

        m_data = JsonUtility.FromJson<AchievementGuids>(json);
    }

    private void OnDisable()
    {
        AnalyticsEvent.Custom("ShipPartChoice", new Dictionary<string, object>()
        {
            {"body",m_itemCurrentlySelected},
            {"weapon","none"},
            
        });
    }

    private void UpdateCustomizationItems(AchievementGuids data)
    {
        foreach (var item in m_shipCustomizationItems)
        {
            item.Lock();
            if (data == null) return;
            
            if (data.m_achievementGuids.Any(guid => item.m_achievement.m_guid == guid))
            {
                item.Unlock();
            }
        }
        
        foreach (var item in m_weaponCustomizationItems)
        {
            item.Lock();
            if (data == null) return;
            
            if (data.m_achievementGuids.Any(guid => item.m_achievement.m_guid == guid))
            {
                item.Unlock();
            }
        }
    }
}

public class OnCustomisationSelection : UnityEvent<Material>
{
}
