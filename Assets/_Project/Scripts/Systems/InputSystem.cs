using Unity.Entities;
using UnityEngine;

namespace _Project.Scripts.Systems
{
    public class InputSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref InputDataComponent _input) =>
            {
                _input.m_inputLeft = Input.GetKey(KeyCode.Q)?1:0;
                _input.m_inputRight = Input.GetKey(KeyCode.D)?1:0;
                _input.m_inputForward = Input.GetKey(KeyCode.Z)?1:0;
                
            }).Run();
        }
    }
}
