using _Project.Scripts.Components;
using Unity.Entities;

namespace _Project.Scripts.Systems
{
    public class PrepareForDestructionIfLifeTimeIsOverSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            Entities.ForEach((Entity _entity, ref LifeTimeComponentData _lifeTime, ref DestroyableComponentData _destroyableComponent) =>
            {
                _lifeTime.m_currentLifeTime += deltaTime;
            
                if (_lifeTime.m_currentLifeTime >= _lifeTime.m_maxLifeTime && _destroyableComponent.m_mustBeDestroyed == false)
                {
                    _destroyableComponent.m_mustBeDestroyed = true;
                }
            }).ScheduleParallel();
        }
    }
}
