using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Components;

using Unity.Entities;
using Unity.Transforms;

public class AsteroidDestructionSystem : SystemBase
{

    protected override void OnCreate()
    {
        base.OnCreate();
        
    }

    protected override void OnUpdate()
    {
        var entityManager = World.EntityManager;
        Entities.WithoutBurst().WithStructuralChanges().WithAll<AsteroidTagComponent>().ForEach((Entity _entity,
            in DestroyableComponentData _destroyable, in PointsComponentData _points, in Translation _translation) =>
        {
            if (_destroyable.m_mustBeDestroyed)
            {
                //ScoreManager.AddPoints(_points.m_points);
                entityManager.DestroyEntity(_entity);
                var fx = FXPool.GetExplosionFx();
                fx.transform.position = _translation.Value;
            }
        }).Run();
    }
}