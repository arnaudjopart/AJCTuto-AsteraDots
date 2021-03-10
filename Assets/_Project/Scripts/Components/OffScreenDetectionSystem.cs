
using _Project.Scripts.Components;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class OffScreenDetectionSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var screenData = GetSingleton<ScreenDataComponent>();

        Entities.WithAll<OffScreenWrapperComponent>().WithAll<MovementCommandsComponentData>().ForEach(
            (Entity _entity, ref OffScreenWrapperComponent _offScreen,
                ref MovementCommandsComponentData _moveComponent, ref Translation _translation) =>
            {
                var previousPosition = _moveComponent.m_previousPosition;
                var currentPosition = _translation.Value;

                var isMovingLeft = currentPosition.x - previousPosition.x < 0;
                var isMovingRight = currentPosition.x - previousPosition.x > 0;
                var isMovingUp = currentPosition.y - previousPosition.y > 0;
                var isMovingDown = currentPosition.y - previousPosition.y < 0;
                
                if (_translation.Value.x < -(screenData.m_width + _offScreen.m_bounds)* .5f && isMovingLeft)
                {
                    //Debug.Log("Leaving screen on left");
                    //_offScreen.m_isOffScreen = true;
                    _offScreen.m_isOffScreenLeft = true;
                }
                
                if (_translation.Value.x > (screenData.m_width + _offScreen.m_bounds) * .5f && isMovingRight)
                {
                    //Debug.Log("Leaving screen on right");
                    //_offScreen.m_isOffScreen = true;
                    _offScreen.m_isOffScreenRight = true;
                }
                if (_translation.Value.y > (screenData.m_height + _offScreen.m_bounds) * .5f && isMovingUp)
                {
                    //Debug.Log("Leaving screen on top");
                    //_offScreen.m_isOffScreen = true;
                    _offScreen.m_isOffScreenUp = true;
                }
                if (_translation.Value.y < -(screenData.m_height + _offScreen.m_bounds) * .5f && isMovingDown)
                {
                    //Debug.Log("Leaving screen on down");
                    //_offScreen.m_isOffScreen = true;
                    _offScreen.m_isOffScreenDown = true;
                }
            }).Schedule();
    }
}
