using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.ScriptableObjects.Achievements;
using UnityEngine;

public class AchievementBuilder
{
    private AchievementCondition[] m_conditions;
    public AchievementBuilder WithConditions(params AchievementCondition[] _conditions)
    {
        m_conditions = _conditions;
        return this;
    }
    public Achievement Build()
    {
        var achievement = ScriptableObject.CreateInstance<Achievement>();
        achievement.m_conditions = m_conditions;
        return achievement;
    }

    public static implicit operator Achievement (AchievementBuilder _builder)
    {
        return _builder.Build();
    }
}
