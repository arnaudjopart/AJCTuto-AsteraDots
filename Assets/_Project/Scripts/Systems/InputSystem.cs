using com.ajc.InputSystem;
using Unity.Entities;
using UnityEngine;

namespace _Project.Scripts.Systems
{
    public class InputSystem : SystemBase
    {
        private InputActions m_inputActions;
        private Vector2 m_inputVector;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_inputActions = new InputActions();
            m_inputActions.Gameplay.MoveAndRotate.performed += _context => m_inputVector = _context.ReadValue<Vector2>();
        }

        protected override void OnStartRunning()
        {
            base.OnStartRunning();
            m_inputActions.Enable();
        }

        protected override void OnDestroy()
        {
            m_inputActions.Disable();
        }

        protected override void OnUpdate()
        {
            var vector = m_inputVector;
            
            Entities.ForEach((ref InputComponentData _input) =>
            {
                
                _input.m_inputLeft = vector.x<0;
                _input.m_inputRight = vector.x>0;
                _input.m_inputForward = vector.y>0;
                
            }).Run();
        }
    }
}