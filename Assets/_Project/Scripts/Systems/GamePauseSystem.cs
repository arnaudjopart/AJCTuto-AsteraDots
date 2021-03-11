using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using _Project.Scripts.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

public class GamePauseSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem m_endOfSym;

    protected override void OnCreate()
    {
        base.OnCreate();
        m_endOfSym = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var query = GetEntityQuery(typeof(GameStateDataComponent));
        
        var component = query.ToComponentDataArray<GameStateDataComponent>(Allocator.TempJob);
        if (component.Length == 0)
        {
            component.Dispose();
            return;
        }
        var gameData = component[0];

        var ecb = m_endOfSym.CreateCommandBuffer().AsParallelWriter();

        if (gameData.m_isOnPause == false)
        {
            Entities.WithAll<MovementCommandsComponentData>().WithAll<PauseMovementDataComponent>().ForEach((
                Entity _entity, int entityInQueryIndex,
                ref MovementCommandsComponentData _movementCommands, 
                ref PhysicsVelocity _velocity, 
                ref PauseMovementDataComponent _pauseMovementData) =>
            {
                _movementCommands.m_angularImpulse = _pauseMovementData.m_savedAngularImpulse;
                _movementCommands.m_linearImpulseCommand = _pauseMovementData.m_savedLinearImpulse;

                _velocity.Linear = _pauseMovementData.m_savedLinearVelocity;
                _velocity.Angular = _pauseMovementData.m_savedAngularVelocity;

                ecb.RemoveComponent<PauseMovementDataComponent>(entityInQueryIndex, _entity);

            }).ScheduleParallel();
            component.Dispose();
            m_endOfSym.AddJobHandleForProducer(Dependency);
            
        }
        else
        {
            Entities.WithAll<MovementCommandsComponentData>().WithNone<PauseMovementDataComponent>().ForEach((
                Entity _entity, int entityInQueryIndex,
                ref MovementCommandsComponentData _movementCommands, ref PhysicsVelocity _velocity) =>
            {
                var currentAngularImpulse = _movementCommands.m_angularImpulse;
                var currentLinearImpulse = _movementCommands.m_linearImpulseCommand;
            
            
            
                ecb.AddComponent(entityInQueryIndex, _entity, new PauseMovementDataComponent()
                {
                    m_savedLinearVelocity = _velocity.Linear,
                    m_savedAngularVelocity = _velocity.Angular,
                    m_savedAngularImpulse = currentAngularImpulse,
                    m_savedLinearImpulse = currentLinearImpulse
                });
            
                _movementCommands.m_angularImpulse = 0;
                _movementCommands.m_linearImpulseCommand = 0;

                _velocity.Angular = float3.zero;
                _velocity.Linear = float3.zero;

            }).ScheduleParallel();
            component.Dispose();
            m_endOfSym.AddJobHandleForProducer(Dependency);
        }
        
        

    }
}