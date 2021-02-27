using _Project.Scripts.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;

namespace _Project.Scripts.Systems
{
    public class MoveSystem : SystemBase
    {
   
        protected override void OnUpdate()
        {
            Entities.ForEach((
                ref MovementInfoComponent _movementInfoComponent,
                ref PhysicsVelocity _velocity,
                in MovementParametersComponentData _moveComponentData,  
                in PhysicsMass _physicsMass) =>
            {
                PhysicsComponentExtensions.ApplyLinearImpulse(
                    ref _velocity, 
                    _physicsMass, 
                    _movementInfoComponent.m_directionOfMove * _movementInfoComponent.m_linearImpulse* _moveComponentData.m_physicsLinearImpulse);
            
                if (math.length(_velocity.Linear) > _moveComponentData.m_maxLinearVelocity) 
                    _velocity.Linear = math.normalize(_velocity.Linear) * _moveComponentData.m_maxLinearVelocity;
            
            }).Schedule();

        }
    }
}