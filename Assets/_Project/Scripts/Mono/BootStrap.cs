using _Project.Scripts.Components;
using Unity.Entities;
using UnityEngine;

namespace _Project.Scripts.Mono
{
    public class BootStrap : MonoBehaviour
    {
        private EntityManager m_entityManager;

        private void Awake()
        {
            
            m_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            
        }


        void Start()
        { 
            
            m_entityManager.CreateEntity(typeof(InputComponentData));
        
        }
    }
}
