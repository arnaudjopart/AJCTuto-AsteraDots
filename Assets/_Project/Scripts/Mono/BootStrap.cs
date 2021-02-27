using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using com.ajc.input;
using Unity.Mathematics;

public class BootStrap : MonoBehaviour
{
    private EntityManager m_entityManager;
    private Entity m_inputEntity;
    private Vector2 m_move;
    private Controls m_gameplayActions;

    private void Awake()
    {
        m_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        m_gameplayActions = new Controls();
        m_gameplayActions.Gameplay.Movement.performed += _context => m_move = _context.ReadValue<Vector2>();

    }

    private void OnEnable()
    {
        m_gameplayActions.Enable();
    }

    private void Move()
    {
        
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
        
        inputData.m_inputLeft = m_move.x<0 ?1:0;
        inputData.m_inputRight = m_move.x>0 ?1:0;
        inputData.m_inputForward = m_move.y>0?1:0;
        
        m_entityManager.SetComponentData(m_inputEntity,inputData);
    }
}
