using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Mono;
using Unity.Entities;
using UnityEngine;

public class AsteroidLibrary_Authoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public BootStrap m_bootStrap;
    public GameObject[] m_asteroidsPrefabCollection;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var buffer = dstManager.AddBuffer<EntityBufferElement>(entity);

        foreach (var VARIABLE in m_asteroidsPrefabCollection)
        {
            buffer.Add(new EntityBufferElement()
            {
                m_entity = conversionSystem.GetPrimaryEntity(VARIABLE)
            });
        }

        m_bootStrap.m_asteroidEntityLibrary = entity;
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.AddRange(m_asteroidsPrefabCollection);
    }
}


public struct EntityBufferElement : IBufferElementData
{
    public Entity m_entity;
}