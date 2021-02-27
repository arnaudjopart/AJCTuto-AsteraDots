
using Unity.Collections;
using Unity.Entities;


namespace _Project.Scripts.Systems
{
    public class PlayerControlSystem : SystemBase
    {

        protected override void OnUpdate()
        {
            var query = GetEntityQuery(typeof(InputDataComponent));
            var array = query.ToComponentDataArray<InputDataComponent>(Allocator.TempJob);

            var inputData = array[0];
            
            Entities.WithAll<PlayerTagComponent>().ForEach((ref MovementParametersComponent _movementParametersComponent) =>
            {
                var rotationDirection = inputData.m_inputLeft - inputData.m_inputRight;
                
                _movementParametersComponent.m_angularImpulse = rotationDirection;
                _movementParametersComponent.m_linearImpulse = inputData.m_inputForward;

            }).ScheduleParallel();
            array.Dispose();
        }
    }
}