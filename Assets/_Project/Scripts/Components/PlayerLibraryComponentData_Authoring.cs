using System.Collections.Generic;
using _Project.Scripts.Mono;
using Unity.Entities;
using UnityEngine;

namespace _Project.Scripts.Components
{
    public class PlayerLibraryComponentData_Authoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    {
        public BootStrap m_bootStrap;
        public GameObject m_shipPrefab;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new ShipLibraryElementComponentData
            {
                m_ship = conversionSystem.GetPrimaryEntity(m_shipPrefab)
            });

            m_bootStrap.m_playerShipEntity = entity;
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(m_shipPrefab);
        }
    }

    public struct ShipLibraryElementComponentData : IComponentData
    {
        public Entity m_ship { get; set; }
    }
}
