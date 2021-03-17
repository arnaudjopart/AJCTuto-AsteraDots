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
            if (true)
            {
                if (_offScreenWrapperComponent.m_isOffScreenLeft)
                {
                    _offScreenWrapperComponent.m_hasWrapAtLeastOnce = true;
                    var newXPosition = SpawnOnRightSide(_translation.Value.x, 
                        _offScreenWrapperComponent.m_bounds,
                        screenDataComponent);
                    var newPosition = new float3(newXPosition, _translation.Value.y, 0);
                    _translation.Value = newPosition;
                }
                else if (_offScreenWrapperComponent.m_isOffScreenRight)
                {
                    _offScreenWrapperComponent.m_hasWrapAtLeastOnce = true;
                    var newXPosition = SpawnOnLeftSide(_translation.Value.x, 
                        _offScreenWrapperComponent.m_bounds,
                        screenDataComponent);
                    var newPosition = new float3(newXPosition, _translation.Value.y, 0);
                    _translation.Value = newPosition;
                }
                else if (_offScreenWrapperComponent.m_isOffScreenUp)
                {
                    _offScreenWrapperComponent.m_hasWrapAtLeastOnce = true;
                    var newYPosition = SpawnOnBottomSide(_translation.Value.y, 
                        _offScreenWrapperComponent.m_bounds,
                        screenDataComponent);
                    var newPosition = new float3(_translation.Value.x, newYPosition, 0);
                    _translation.Value = newPosition;
                }
                else if (_offScreenWrapperComponent.m_isOffScreenDown)
                {
                    _offScreenWrapperComponent.m_hasWrapAtLeastOnce = true;
                    var newYPosition = SpawnOnTopSide(_translation.Value.y, 
                        _offScreenWrapperComponent.m_bounds,
                        screenDataComponent);
                    var newPosition = new float3(_translation.Value.x, newYPosition, 0);
                    _translation.Value = newPosition;
                }
                
            }

            //_offScreenWrapperComponent.m_isOffScreen = false;
            _offScreenWrapperComponent.m_isOffScreenDown = false;
            _offScreenWrapperComponent.m_isOffScreenRight = false;
            _offScreenWrapperComponent.m_isOffScreenUp = false;
            _offScreenWrapperComponent.m_isOffScreenLeft = false;
            
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
