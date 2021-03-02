
using _Project.Scripts.Components;
using Unity.Collections;
using Unity.Entities;

namespace _Project.Scripts.Systems
{
    public class PlayerControlSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var query = GetEntityQuery(typeof(InputComponentData));
            var array = query.ToComponentDataArray<InputComponentData>(Allocator.TempJob);

            var inputData = array[0];
            
            Entities.WithAll<PlayerTagComponent>().ForEach((ref MovementInfoComponent _movementInfoComponent, 
                ref WeaponComponentData _weaponComponent) =>
            {
                var turningLeft = inputData.m_inputLeft ? 1 : 0;
                var turningRight = inputData.m_inputRight ? 1 : 0;
                
                var rotationDirection = turningLeft - turningRight;

                _weaponComponent.m_isFiring = inputData.m_inputShoot;
                
                _movementInfoComponent.m_angularImpulse = rotationDirection;
                _movementInfoComponent.m_linearImpulse = inputData.m_inputForward?1:0;

            }).ScheduleParallel();
            
            array.Dispose();
        }
    }
}