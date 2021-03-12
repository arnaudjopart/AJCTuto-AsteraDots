using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.ScriptableObjects.Achievements;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class AchievementTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void AchievementTestSimplePasses()
    {
        var achievement = ScriptableObject.CreateInstance<AchievementScriptableObject>();
        achievement.m_isUnlocked = true;
        Assert.AreEqual(false, achievement.m_isUnlocked);
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
