
using _Project.Scripts.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace _Project.Scripts.Systems
{
    public class PlayerControlSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var query = GetEntityQuery(typeof(InputComponentData));
            var array = query.ToComponentDataArray<InputComponentData>(Allocator.TempJob);

            if (array.Length == 0)
            {
                array.Dispose();
                return;
            }
            var inputData = array[0];
            
            Entities.WithAll<PlayerTagComponent>().WithNone<PauseMovementDataComponent>().
                ForEach((ref MovementCommandsComponentData _movementInfoComponent,ref WeaponComponentData _weaponComponent) =>
            {
                var turningLeft = inputData.m_inputLeft ? 1 : 0;
                var turningRight = inputData.m_inputRight ? 1 : 0;
                
                var rotationDirection = turningLeft - turningRight;

                _weaponComponent.m_isFiring = inputData.m_inputShoot;
                
                _movementInfoComponent.m_angularVector = new float3( 0,0,rotationDirection);
                _movementInfoComponent.m_linearImpulseCommand = inputData.m_inputForward?1:0;

            }).ScheduleParallel();
            
            array.Dispose();
        }
    }
}
