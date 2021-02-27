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
                ref MovementInfoComponent _movementParametersComponent, 
                ref Rotation _rotation) =>
            {
                var direction = math.mul(_rotation.Value, math.up());
                _movementParametersComponent.m_directionOfMove = direction;

            }).Schedule();
        }
    }
}