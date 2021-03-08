using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class OffScreenWrappingSystem : SystemBase
{
    private EntityCommandBuffer m_entityCommandBuffer;

    private enum OFFSCREENSIDE
    {
        LEFT,RIGHT,UP,DOWN
    }

    protected override void OnCreate()
    {
        base.OnCreate();
            //m_entityCommandBuffer = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EntityCommandBuffer>();
    }

    protected override void OnUpdate()
    {
        var screenDataComponent = GetSingleton<ScreenDataComponent>();
        
        Entities.WithAll<OffScreenWrapperComponent>().ForEach((
            Entity _entity, 
            ref OffScreenWrapperComponent _offScreenWrapperComponent, 
            ref Translation _translation) => {
            if (_offScreenWrapperComponent.m_isOffScreen)
            {
                if (_translation.Value.x < -screenDataComponent.m_width * .5f)
                {
                    var newXPosition = SpawnOnRightSide(_translation.Value.x, 
                        _offScreenWrapperComponent.m_bounds,
                        screenDataComponent);
                    var newPosition = new float3(newXPosition, _translation.Value.y, 0);
                    _translation.Value = newPosition;
                }
                else if (_translation.Value.x > screenDataComponent.m_width * .5f)
                {
                    var newXPosition = SpawnOnLeftSide(_translation.Value.x, 
                        _offScreenWrapperComponent.m_bounds,
                        screenDataComponent);
                    var newPosition = new float3(newXPosition, _translation.Value.y, 0);
                    _translation.Value = newPosition;
                }
                else if (_translation.Value.y > screenDataComponent.m_height * .5f)
                {
                    var newYPosition = SpawnOnBottomSide(_translation.Value.y, 
                        _offScreenWrapperComponent.m_bounds,
                        screenDataComponent);
                    var newPosition = new float3(_translation.Value.x, newYPosition, 0);
                    _translation.Value = newPosition;
                }
                else if (_translation.Value.y < -screenDataComponent.m_height * .5f)
                {
                    var newYPosition = SpawnOnTopSide(_translation.Value.y, 
                        _offScreenWrapperComponent.m_bounds,
                        screenDataComponent);
                    var newPosition = new float3(_translation.Value.x, newYPosition, 0);
                    _translation.Value = newPosition;
                }
                
            }

            _offScreenWrapperComponent.m_isOffScreen = false;
            
        }).ScheduleParallel();
    }

    private static float SpawnOnRightSide(float _valueX, float _bounds, ScreenDataComponent _screenDataComponent)
    {
        return (_bounds + _screenDataComponent.m_width)*.5f;
    }
    private static float SpawnOnLeftSide(float _valueX, float _bounds, ScreenDataComponent _screenDataComponent)
    {
        return  -(_bounds + _screenDataComponent.m_width)*.5f;
    }
    private static float SpawnOnTopSide(float _valueY, float _bounds, ScreenDataComponent _screenDataComponent)
    {
        return  (_bounds + _screenDataComponent.m_height)*.5f;
    }
    private static float SpawnOnBottomSide(float _valueY, float _bounds, ScreenDataComponent _screenDataComponent)
    {
        return  - (_bounds + _screenDataComponent.m_height)*.5f;
    }
}
