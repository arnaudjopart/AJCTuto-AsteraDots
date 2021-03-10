using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Mono;
using _Project.Scripts.ScriptableObjects;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameEvent m_onLevelCompletedEvent;
    
    public LevelDataScriptableObject[] m_levelDataScriptableObjects;

    private LevelDataScriptableObject m_currentLevelData;

    public int m_hits;

    public int m_currentLevelIndex=-1;

    public BootStrap m_bootStrap;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNextLevel()
    {
        m_currentLevelIndex++;
        m_currentLevelData = m_levelDataScriptableObjects[m_currentLevelIndex];
        
    }

    public void AddHit()
    {
        m_hits++;
        if (m_hits >= m_currentLevelData.NbOfHits)
        {
            print("LevelCompleted");
            m_onLevelCompletedEvent.Raise();
        }
    }
}
