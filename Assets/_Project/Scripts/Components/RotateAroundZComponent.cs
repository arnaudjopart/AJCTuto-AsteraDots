using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct RotateAroundZComponent : IComponentData
{
    public float m_speed;
}
