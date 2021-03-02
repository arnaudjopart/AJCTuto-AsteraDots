using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct AutoDestroyAfterSeconds : IComponentData
{
    public float m_lifeTime;
    public float m_lifeSpent;
}
