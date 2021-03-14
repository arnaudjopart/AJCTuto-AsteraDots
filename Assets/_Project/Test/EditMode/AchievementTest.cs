using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Mono;
using _Project.Scripts.ScriptableObjects.Achievements;
using _Project.Test;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class AchievementTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void Achievement_Based_On_Score_And_Levels_Fails_if_Score_Not_High_Enough_But_Level_is_Reached()
    {
        var gameData = new GameData()
        {
            CurrentScore = 100,
            CurrentLevel = 2
        };
        
        AchievementCondition scoreCondition = An.AchievementCondition()
            .WithType(AchievementCondition.TYPE.SCORE)
            .WithThreshold(1000);
        
        AchievementCondition levelCondition = An.AchievementCondition()
            .WithType(AchievementCondition.TYPE.LEVEL)
            .WithThreshold(1);

        Achievement achievement = An.Achievement().WithConditions(scoreCondition,levelCondition);

        var unlock = achievement.IsAchievementUnlocked(gameData);
        Assert.AreEqual(false, unlock);
    }
    
    [Test]
    public void Achievement_Based_On_Score_And_Levels_Succeed_if_Score_is_High_Enough_And_Level_is_Reached()
    {
        var gameData = new GameData()
        {
            CurrentScore = 1000,
            CurrentLevel = 2
        };
        
        AchievementCondition scoreCondition = An.AchievementCondition()
            .WithType(AchievementCondition.TYPE.SCORE)
            .WithThreshold(100);
        
        AchievementCondition levelCondition = An.AchievementCondition()
            .WithType(AchievementCondition.TYPE.LEVEL)
            .WithThreshold(1);

        Achievement achievement = An.Achievement().WithConditions(scoreCondition,levelCondition);

        var unlock = achievement.IsAchievementUnlocked(gameData);
        Assert.AreEqual(true, unlock);
    }
    
    [Test]
    public void Achievement_Based_On_Score_Success_if_Score_Is_High_Enough()
    {
        var gameData = new GameData()
        {
            CurrentScore = 1000
        };
        

        AchievementCondition condition = An.AchievementCondition()
            .WithType(AchievementCondition.TYPE.SCORE)
            .WithThreshold(100);

        Achievement achievement = An.Achievement().WithConditions(condition);
        
        var unlock = achievement.IsAchievementUnlocked(gameData);
        Assert.AreEqual(true, unlock);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator AchievementTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
