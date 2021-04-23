using Unity.Entities;

namespace _Project.Scripts.Components
{
    [GenerateAuthoringComponent]
    public struct DestroyableComponentData : IComponentData
    {
        public bool m_mustBeDestroyed;
    }
}