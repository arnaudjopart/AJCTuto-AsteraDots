
using Unity.Entities;

public struct InputDataComponent : IComponentData
{
    public bool m_inputLeft;
    public bool m_inputRight;
    public bool m_inputForward;
}
