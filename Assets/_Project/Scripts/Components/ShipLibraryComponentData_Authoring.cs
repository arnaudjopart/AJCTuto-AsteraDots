using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Mono;
using Unity.Entities;
using UnityEngine;

public class ShipLibraryComponentData_Authoring : MonoBehaviour, IConvertGameObjectToEntity,IDeclareReferencedPrefabs
{
    public BootStrap m_bootStrap;
    public GameObject m_prefab;
    
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new ShipReferenceInBoostrapComponentData()
        {
            m_ship = conversionSystem.GetPrimaryEntity(m_prefab)
        });
        m_bootStrap.m_playerLibrary = entity;
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(m_prefab);
    }
}

public struct ShipReferenceInBoostrapComponentData : IComponentData
{
    public Entity m_ship { get; set; }
}

