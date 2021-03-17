using Unity.Entities;
using UnityEngine;

namespace _Project.Scripts.Components
{
    public class ScreenWrapperComponentData_Authoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public MeshRenderer m_mesh;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponent(entity, typeof(OffScreenWrapperComponent));
            dstManager.SetComponentData(entity, new OffScreenWrapperComponent
            {
                m_bounds = m_mesh.bounds.extents.magnitude
            });
        }
    }

    public struct OffScreenWrapperComponent : IComponentData
    {
        public float m_bounds;
        //public bool m_isOffScreen;
        public bool m_isOffScreenLeft;
        public bool m_isOffScreenRight;
        public bool m_isOffScreenDown;
        public bool m_isOffScreenUp;

        public bool m_hasWrapAtLeastOnce;
    }
}
