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

        PlayerHealth damage = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();

        if (damage == null)
        {
            Debug.LogError("Damage component not found.");
            yield break; // Exit the test
        }

        // Store the initial health
        float initialDamage = PlayerHealth.damage;

        // Get a reference to the PlayerHealth component attached to the player GameObject
        PlayerHealth maxHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();

        if (maxHealth == null)
        {
            Debug.LogError("MaxHealth component not found.");
            yield break; // Exit the test
        }

        // Store the initial health
        float initialMaxHealth = PlayerHealth.Instance.MaxHealth;

        // Get a reference to the PlayerHealth component attached to the player GameObject
        TopDownMovement moveSpeed = GameObject.FindGameObjectWithTag("Player").GetComponent<TopDownMovement>();

        if (moveSpeed == null)
        {
            Debug.LogError("Player Speed component not found.");
            yield break; // Exit the test
        }

        // Store the initial health
        float initialMoveSpeed = TopDownMovement.moveSpeed;

        // Store the initial regen amount
        float initialRegenAmount = PlayerHealth.Instance.RegenAmount;

        Shooting firerate = GameObject.FindGameObjectWithTag("Player").GetComponent<Shooting>();

        if (firerate == null)
        {
            Debug.LogError("Firerate component not found.");
            yield break; // Exit the test
        }

        // Store the initial firerate
        float initialFirerate = Shooting.firerate;

        EnemyHealth bleedAmount = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyHealth>();

        if (bleedAmount == null)
        {
            Debug.LogError("BleedAmount component not found.");
            yield break; // Exit the test
        }

        // Store the initial Bleed amount
        float initialBleedAmount = EnemyHealth.bleedAmount;

        PlayerHealth lifestealAmount = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();

        if (lifestealAmount == null)
        {
            Debug.LogError("LifestealAmount component not found.");
            yield break; // Exit the test
        }

        // Store the initial lifesteal amount
        float initialLifestealAmount = PlayerHealth.lifestealAmount;

        PlayerHealth explosionSize = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();

        if (explosionSize == null)
        {
            Debug.LogError("explosionSize component not found.");
            yield break; // Exit the test
        }

        // Store the initial explosionSize amount
        float initialexplosionSize = PlayerHealth.explosionSize;

        PlayerHealth critChance = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();

        if (critChance == null)
        {
            Debug.LogError("explosionSize component not found.");
            yield break; // Exit the test
        }

        // Store the initial initialCritChance amount
        float initialCritChance = PlayerHealth.critChance;

        PlayerHealth bulletAmount = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();

        if (bulletAmount == null)
        {
            Debug.LogError("explosionSize component not found.");
            yield break; // Exit the test
        }

        // Store the initial initialShotgun amount
        float initialShotgun = PlayerHealth.bulletAmount;

       // Wait for the stats to be updated
        yield return new WaitForSeconds(10f);
        Assert.Greater(PlayerHealth.damage, initialDamage, "Damage did not increase");
        Assert.Greater(PlayerHealth.Instance.MaxHealth, initialMaxHealth, "MaxHealth did not increase");
        Assert.Greater(TopDownMovement.moveSpeed, initialMoveSpeed, "Speed did not increase");
        Assert.Greater(PlayerHealth.Instance.RegenAmount, initialRegenAmount, "Regen Amount did not increase");
        Assert.Less(Shooting.firerate, initialFirerate, "Firerate did not decrease");
        Assert.Greater(EnemyHealth.bleedAmount, initialBleedAmount, "BleedAmount did not increase");
        Assert.Greater(PlayerHealth.lifestealAmount, initialLifestealAmount, "LifestealAmount did not increase");
        Assert.Greater(PlayerHealth.explosionSize, initialexplosionSize, "ExplosionSize did not increase");
        Assert.Greater(PlayerHealth.critChance, initialCritChance, "CritChance did not increase");
        Assert.Greater(PlayerHealth.bulletAmount, initialShotgun, "initialShotgun did not increase");
    }
}

