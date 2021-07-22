using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Components;
using Unity.Entities;
using UnityEngine;

public class ShipDestructionSystem : SystemBase
{
    private EntityManager m_entityManager;

    protected override void OnCreate()
    {
        base.OnCreate();
        m_entityManager = World.EntityManager;
    }

    protected override void OnUpdate()
    {
        Entities
            .WithStructuralChanges()
            .WithoutBurst()
            .WithAll<PlayerTagComponent>()
            .ForEach((Entity _entity, ref DestroyableComponentData _destroyable) =>
            {
                if (_destroyable.m_mustBeDestroyed)
                {
                    m_entityManager.DestroyEntity(_entity);
                }
            }).Run();

    }
}
