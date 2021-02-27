using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class BootStrap : MonoBehaviour
{
    private EntityManager m_entityManager;
    private Entity m_inputEntity;

    private void Awake()
    {
        m_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    // Start is called before the first frame update
    void Start()
    { 
        m_inputEntity = m_entityManager.CreateEntity(typeof(InputDataComponent));
        
    }

    // Update is called once per frame
    void Update()
    { 
        var inputData = m_entityManager.GetComponentData<InputDataComponent>(m_inputEntity);
        
        inputData.m_inputLeft = Input.GetKey(KeyCode.Q)?1:0;
        inputData.m_inputRight = Input.GetKey(KeyCode.D)?1:0;
        inputData.m_inputForward = Input.GetKey(KeyCode.Z)?1:0;
        
        m_entityManager.SetComponentData(m_inputEntity,inputData);
    }
}
