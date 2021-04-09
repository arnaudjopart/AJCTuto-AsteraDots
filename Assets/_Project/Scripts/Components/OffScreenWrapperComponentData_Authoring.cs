using Unity.Entities;
using UnityEngine;

namespace _Project.Scripts.Components
{
    public class OffScreenWrapperComponentData_Authoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public MeshRenderer m_mesh;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new OffScreenWrapperComponentData
            {
                m_bounds = m_mesh.bounds.extents.magnitude
            });
        }
    }
}