using _Project.Scripts.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;

namespace _Project.Scripts.Systems
{
    public class RotateSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((
                ref PhysicsVelocity _velocity, ref MovementCommandsComponentData _commandsComponentData,
                ref Rotation _rotation, in MovementParametersComponentData _moveComponentData, in PhysicsMass _physicsMass) =>
            {
                PhysicsComponentExtensions.ApplyAngularImpulse(
                    ref _velocity, _physicsMass,
                    _commandsComponentData.m_angularVector * _moveComponentData.m_angularVelocity);

                var currentAngularSpeed = PhysicsComponentExtensions.GetAngularVelocityWorldSpace(in _velocity, in _physicsMass, in _rotation);
            
                if(math.length(currentAngularSpeed)>_moveComponentData.m_maxAngularVelocity)
                {
                    PhysicsComponentExtensions.SetAngularVelocityWorldSpace(ref _velocity, _physicsMass, _rotation,
                        math.normalize(currentAngularSpeed)* _moveComponentData.m_maxAngularVelocity );
                }
            
            }).Schedule();
        }
    }
}
