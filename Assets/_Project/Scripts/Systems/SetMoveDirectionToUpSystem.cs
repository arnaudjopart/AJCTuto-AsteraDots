using _Project.Scripts.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace _Project.Scripts.Systems
{
    public class SetMoveDirectionToUpSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.WithAll<MovingInUpDirectionComponent>().ForEach((
                ref MovementCommandsComponentData _movementCommandsComponentData, 
                in Rotation _rotation) =>
            {
                var direction = math.mul(_rotation.Value, math.up());
                _movementCommandsComponentData.m_currentDirectionOfMove = direction;

            }).Schedule();
        }
    }
}