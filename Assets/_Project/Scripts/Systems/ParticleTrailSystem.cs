using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace _Project.Scripts.Systems
{
    public class ParticleTrailSystem : SystemBase
    {
        private EntityManager m_entityManager;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_entityManager = World.EntityManager;
        }

        protected override void OnUpdate()
        {
            Entities.WithStructuralChanges().WithAll<ParticleEffectLink>().WithNone<EffectIDSystemState>().ForEach((Entity _entity)=>
            {
                GameObject obj = FXPool.GetFx();

                m_entityManager.AddComponentObject(_entity, obj.transform);
                m_entityManager.AddComponentData(_entity, new CopyTransformToGameObject() { });
                m_entityManager.AddComponentData(_entity, new EffectIDSystemState() { });
                var test = m_entityManager.GetComponentObject<Transform>(_entity);
                //Object.Destroy(test.gameObject);

            }).Run();
        
        }
    }


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
            Entities.WithStructuralChanges().WithAll<ParticleEffectLink>().WithAll<EffectIDSystemState>().ForEach((Entity _entity, 
                ref AutoDestroyAfterSeconds _destroyAfter)=>
            {
                _destroyAfter.m_lifeSpent += deltaTime;
                if (_destroyAfter.m_lifeSpent > _destroyAfter.m_lifeTime)
                {
                    var test = m_entityManager.GetComponentObject<Transform>(_entity);
                    Object.Destroy(test.gameObject);
                    m_entityManager.DestroyEntity(_entity);
                }
                
            }).Run();
        
        }
    }
}