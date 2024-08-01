using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class ItemTesting2
{
    [UnityTest]
    public IEnumerator IncreaseHealth_ItemEffect_Test()
    {
        // Load the MainScene before running the tests
        yield return SceneManager.LoadSceneAsync("ItemScene2");

        // Wait for a moment to ensure player is spawned
        yield return new WaitForSeconds(1f);

        // Get a reference to the StatsCheckerScript in the scene
        StatsCheckerScript statsChecker = UnityEngine.Object.FindObjectOfType<StatsCheckerScript>();

        // Check if StatsCheckerScript is found in the scene
        if (statsChecker == null)
        {
            Debug.LogError("StatsCheckerScript not found.");
            yield break; // Exit the test
        }

        PlayerStats damage = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        if (damage == null)
        {
            Debug.LogError("Damage component not found.");
            yield break; // Exit the test
        }

        // Store the initial health
        float initialDamage = PlayerStats.Instance.Damage;

        // Get a reference to the PlayerStats component attached to the player GameObject
        PlayerStats maxHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        if (maxHealth == null)
        {
            Debug.LogError("MaxHealth component not found.");
            yield break; // Exit the test
        }

        // Store the initial health
        float initialMaxHealth = PlayerStats.Instance.MaxHealth;

        // Get a reference to the PlayerStats component attached to the player GameObject
        TopDownMovement moveSpeed = GameObject.FindGameObjectWithTag("Player").GetComponent<TopDownMovement>();

        if (moveSpeed == null)
        {
            Debug.LogError("Player Speed component not found.");
            yield break; // Exit the test
        }

        // Store the initial health
        float initialMoveSpeed = TopDownMovement.moveSpeed;

        // Store the initial regen amount
        float initialRegenAmount = PlayerStats.Instance.RegenAmount;

        Shooting firerate = GameObject.FindGameObjectWithTag("Player").GetComponent<Shooting>();

        if (firerate == null)
        {
            Debug.LogError("Firerate component not found.");
            yield break; // Exit the test
        }

        // Store the initial firerate
        float initialFirerate = Shooting.Instance.Firerate;

        EnemyHealth bleedAmount = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyHealth>();

        if (bleedAmount == null)
        {
            Debug.LogError("BleedAmount component not found.");
            yield break; // Exit the test
        }

        // Store the initial Bleed amount
        float initialBleedAmount = EnemyHealth.bleedAmount;

        PlayerStats lifestealAmount = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        if (lifestealAmount == null)
        {
            Debug.LogError("LifestealAmount component not found.");
            yield break; // Exit the test
        }

        // Store the initial lifesteal amount
        float initialLifestealAmount = PlayerStats.Instance.LifestealAmount;

        PlayerStats explosionSize = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        if (explosionSize == null)
        {
            Debug.LogError("explosionSize component not found.");
            yield break; // Exit the test
        }

        // Store the initial explosionSize amount
        float initialexplosionSize = PlayerStats.Instance.ExplosionSize;

        PlayerStats critChance = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        if (critChance == null)
        {
            Debug.LogError("explosionSize component not found.");
            yield break; // Exit the test
        }

        // Store the initial initialCritChance amount
        float initialCritChance = PlayerStats.Instance.CritChance;

        PlayerStats bulletAmount = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        if (bulletAmount == null)
        {
            Debug.LogError("explosionSize component not found.");
            yield break; // Exit the test
        }

        // Store the initial initialShotgun amount
        float initialShotgun = PlayerStats.Instance.BulletAmount;

       // Wait for the stats to be updated
        yield return new WaitForSeconds(10f);
        Assert.Greater(PlayerStats.Instance.Damage, initialDamage, "Damage did not increase");
        Assert.Greater(PlayerStats.Instance.MaxHealth, initialMaxHealth, "MaxHealth did not increase");
        Assert.Greater(TopDownMovement.moveSpeed, initialMoveSpeed, "Speed did not increase");
        Assert.Greater(PlayerStats.Instance.RegenAmount, initialRegenAmount, "Regen Amount did not increase");
        Assert.Less(Shooting.Instance.Firerate, initialFirerate, "Firerate did not decrease");
        Assert.Greater(EnemyHealth.bleedAmount, initialBleedAmount, "BleedAmount did not increase");
        Assert.Greater(PlayerStats.Instance.LifestealAmount, initialLifestealAmount, "LifestealAmount did not increase");
        Assert.Greater(PlayerStats.Instance.ExplosionSize, initialexplosionSize, "ExplosionSize did not increase");
        Assert.Greater(PlayerStats.Instance.CritChance, initialCritChance, "CritChance did not increase");
        Assert.Greater(PlayerStats.Instance.BulletAmount, initialShotgun, "initialShotgun did not increase");
    }
}

