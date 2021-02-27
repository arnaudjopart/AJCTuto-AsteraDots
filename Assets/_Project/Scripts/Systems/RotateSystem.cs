using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;

public class RotateSystem : SystemBase
{
    
    protected override void OnUpdate()
    {
        
        Entities.ForEach((ref PhysicsVelocity _velocity, ref PhysicsMass _physicsMass, ref MoveComponentData _moveComponentData, 
            ref MovementParametersComponent _movementParameters, ref Rotation _rotation) =>
        {
            // ReSharper disable once InvokeAsExtensionMethod
            PhysicsComponentExtensions.ApplyAngularImpulse(
                ref _velocity,
                _physicsMass,
                new float3(0,0,_movementParameters.m_angularImpulse * _moveComponentData.m_angularSpeed));

            var currentAngularSpeed = PhysicsComponentExtensions.GetAngularVelocityWorldSpace(in _velocity, in _physicsMass, in _rotation);
            
            if(math.length(currentAngularSpeed)>_moveComponentData.m_maxAngularVelocity)
            {
                PhysicsComponentExtensions.SetAngularVelocityWorldSpace(
                    ref _velocity, 
                    _physicsMass, 
                    _rotation,
                    math.normalize(currentAngularSpeed)* _moveComponentData.m_maxAngularVelocity );
            }
            
        }).Schedule();
    }
}
