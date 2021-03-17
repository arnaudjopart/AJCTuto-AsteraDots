using _Project.Scripts.Components;
using _Project.Scripts.Mono;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace _Project.Scripts.Systems
{
    public class LaserDestructionSystem : SystemBase
    {
        private EntityManager m_entityManager;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_entityManager = World.EntityManager;
        }
        
        protected override void OnUpdate()
        {
            Entities.WithStructuralChanges()
                .WithAll<OffScreenWrapperComponent>()
                .WithAll<ParticleEffectLink>()
                .WithNone<PauseMovementDataComponent>()
                .WithAll<EffectIDSystemState>()
                .ForEach((Entity _entity, 
                    in OffScreenWrapperComponent _offScreenWrapper,
                    in DestroyableComponentData _destroyable)=>
                {
                    if (!_destroyable.m_mustBeDestroyed) return;

                    if (_offScreenWrapper.m_hasWrapAtLeastOnce && _destroyable.m_hasCollidedWithObject)
                    {
                        GameEventManager.RaiseWrapHitEvent();
                    }
                    var test = m_entityManager.GetComponentObject<Transform>(_entity);
                    Object.Destroy(test.gameObject);
                    m_entityManager.DestroyEntity(_entity);
                
                }).Run();
        }
    }
}