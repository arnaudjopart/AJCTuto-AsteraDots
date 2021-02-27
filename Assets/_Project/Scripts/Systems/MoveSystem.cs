using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;

public class MoveSystem : SystemBase
{
   
    protected override void OnUpdate()
    {
        Entities.ForEach((
            ref MovementInfoComponent _movementParametersComponent, 
            ref MovementParametersComponentData _moveComponentData,  
            ref PhysicsVelocity _velocity, 
            ref Rotation _rotation, 
            ref PhysicsMass _physicsMass) =>
        {
            PhysicsComponentExtensions.ApplyLinearImpulse(
                ref _velocity, 
                _physicsMass, 
                _movementParametersComponent.m_directionOfMove * _movementParametersComponent.m_linearImpulse* _moveComponentData.m_linearSpeed);
            
            if (math.length(_velocity.Linear) > _moveComponentData.m_maxLinearVelocity) 
                _velocity.Linear = math.normalize(_velocity.Linear) * _moveComponentData.m_maxLinearVelocity;
            
        }).Schedule();

    }
}