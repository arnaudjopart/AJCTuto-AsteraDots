using _Project.Scripts.Components;
using _Project.Scripts.Mono;
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
            .WithoutBurst().WithAll<PlayerTagComponent>()
            .ForEach((Entity _entity, ref DestroyableComponentData _destroy) =>
        {
            if (_destroy.m_mustBeDestroyed)
            {
                m_entityManager.DestroyEntity(_entity);
                BootStrap.m_instance.LookForPlayerSpawnPosition();
            }
        }).Run();
    }
}