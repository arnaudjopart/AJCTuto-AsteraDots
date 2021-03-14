using _Project.Scripts.Components;
using Unity.Entities;
using UnityEngine;

namespace _Project.Scripts.Systems
{
    public class InputSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref InputComponentData _input) =>
            {
                _input.m_inputLeft = Input.GetKey(KeyCode.Q);
                _input.m_inputRight = Input.GetKey(KeyCode.D);
                _input.m_inputForward = Input.GetKey(KeyCode.Z);
                
            }).Run();
        }
    }
}
