using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Mono;
using Unity.Entities;
using UnityEngine;

public class ShipEntity_Authoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public BootStrap m_bootStrap;
    public GameObject m_shipPrefab;
    
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new ShipReferenceInBoostrapComponentData()
        {
            m_ship = conversionSystem.GetPrimaryEntity(m_shipPrefab)
        });
        m_bootStrap.m_shipReference = entity;
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(m_shipPrefab);
    }
}

public struct ShipReferenceInBoostrapComponentData : IComponentData
{
    public Entity m_ship;
}
