using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;

public class MoveSystem : SystemBase
{
   
    protected override void OnUpdate()
    {
        Entities.ForEach((ref MovementParametersComponent _movementParametersComponent, 
            ref MoveComponentData _moveComponentData,  ref PhysicsVelocity _velocity, ref Rotation _rotation, ref PhysicsMass _physicsMass) =>
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