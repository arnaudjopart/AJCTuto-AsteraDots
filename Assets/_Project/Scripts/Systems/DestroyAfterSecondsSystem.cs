using _Project.Scripts.Components;
using Unity.Entities;
using UnityEngine;

namespace _Project.Scripts.Systems
{
    public class DestroyAfterSecondsSystem : SystemBase
    {
        private EntityManager m_entityManager;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_entityManager = World.EntityManager;
        }

        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            Entities
                .ForEach((Entity _entity, 
                ref AutoDestroyAfterSeconds _destroyAfter, 
                ref DestroyableComponentData _destroyable)=>
            {
                _destroyAfter.m_lifeSpent += deltaTime;
                if (_destroyAfter.m_lifeSpent > _destroyAfter.m_lifeTime)
                {
                    _destroyable.m_mustBeDestroyed = true;
                }
                
            }).Run();
        
        }
    }
}