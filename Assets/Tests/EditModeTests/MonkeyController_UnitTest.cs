using NUnit.Framework;
using ServiceLocator.Player;
using ServiceLocator.Player.Projectile;
using ServiceLocator.Sound;
using ServiceLocator.Wave.Bloon;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyController_UnitTest
{
    private MonkeyController monkeyController;
    private SoundService soundService;
    private PlayerService playerService;

    [SetUp]
    public void Setup()
    {
        MonkeyScriptableObject monkeySO = CreateDummyMonkeySO();
        ProjectilePool projectilePool = CreateDummyProjectilePool();
        monkeyController = new MonkeyController(monkeySO, projectilePool, soundService);
    }

    private MonkeyScriptableObject CreateDummyMonkeySO()
    {
        MonkeyScriptableObject monkeySO = ScriptableObject.CreateInstance<MonkeyScriptableObject>();
        monkeySO.AttackableBloons = new List<BloonType>();
        monkeySO.AttackableBloons.Add(BloonType.Red);
        monkeySO.Range = 5;
        monkeySO.AttackRate = 1;
        MonkeyView monkeyPrefab = new GameObject().AddComponent<MonkeyView>();
        monkeyPrefab.gameObject.AddComponent<CircleCollider2D>();
        monkeyPrefab.gameObject.AddComponent<Animator>();
        monkeyPrefab.RangeSpriteRenderer = new GameObject().AddComponent<SpriteRenderer>();
        monkeySO.Prefab = monkeyPrefab;
        return monkeySO;
    }

    private ProjectilePool CreateDummyProjectilePool()
    {
        ProjectileView projectilePrefab = new GameObject().AddComponent<ProjectileView>();
        List<ProjectileScriptableObject> projectileSOs = new List<ProjectileScriptableObject>();
        projectileSOs.Add(ScriptableObject.CreateInstance<ProjectileScriptableObject>());
        return new ProjectilePool(projectilePrefab, projectileSOs, playerService);
    }

    [Test]
    public void CanAttackBloon_CheckCondition()
    {
        // Act
        bool canAttack = monkeyController.CanAttackBloon(BloonType.Red);

        // Assert
        Assert.IsTrue(canAttack);
    }

}