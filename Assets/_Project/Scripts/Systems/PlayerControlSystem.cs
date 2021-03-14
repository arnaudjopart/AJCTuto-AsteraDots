
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

            var inputData = array[0];
            
            Entities.WithAll<PlayerTagComponent>().ForEach((ref MovementCommandsComponentData _movementCommandsComponentData) =>
            {
                var turningLeft = inputData.m_inputLeft ? 1 : 0;
                var turningRight = inputData.m_inputRight ? 1 : 0;
                
                var rotationDirection = turningLeft - turningRight;
                
                _movementCommandsComponentData.m_currentAngularCommand = new float3(0,0,rotationDirection);
                _movementCommandsComponentData.m_currentlinearCommand = inputData.m_inputForward?1:0;

            }).ScheduleParallel();
            
            array.Dispose();
        }
    }
}