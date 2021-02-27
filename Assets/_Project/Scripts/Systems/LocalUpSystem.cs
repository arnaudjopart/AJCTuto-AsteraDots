using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class LocalUpSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.WithAll<MovingUpComponent>().ForEach((ref MovementParametersComponent _movementParametersComponent, ref Rotation _rotation) =>
        {
            var direction = math.mul(_rotation.Value, new float3(0, 1, 0));
            _movementParametersComponent.m_directionOfMove = direction;

        }).Schedule();
    }
}