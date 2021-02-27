using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class BootStrap : MonoBehaviour
{
    private EntityManager m_entityManager;

    private void Awake()
    {
        m_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    // Start is called before the first frame update
    void Start()
    { 
        m_entityManager.CreateEntity(typeof(InputDataComponent));
        
    }
    
}
