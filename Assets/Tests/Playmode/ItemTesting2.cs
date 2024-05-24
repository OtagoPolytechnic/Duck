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
        yield return SceneManager.LoadSceneAsync("MainScene");

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

        // Get a reference to the PlayerHealth component attached to the player GameObject
        PlayerHealth playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();

        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth component not found.");
            yield break; // Exit the test
        }

        // Store the initial health
        float initialHealth = PlayerHealth.currentHealth;




        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth component not found.");
            yield break; // Exit the test
        }

        // Store the initial damage
        float initialDamage = PlayerHealth.damage;


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
        float initialRegenAmount = PlayerHealth.regenAmount;



        // Get a reference to the PlayerHealth component attached to the player GameObject
        Shooting firerate = GameObject.FindGameObjectWithTag("Player").GetComponent<Shooting>();

        if (firerate == null)
        {
            Debug.LogError("PlayerHealth component not found.");
            yield break; // Exit the test
        }


        // Store the initial health
        float initialFirerate = Shooting.firerate;

        // Wait for the stats to be updated
        yield return new WaitForSeconds(10f);


        // Assert that the updated health is greater than the initial health
        Assert.Greater(PlayerHealth.maxHealth, initialHealth, "Health did not increase");
        //Debug.Log(initialHealth);
        //Debug.Log(PlayerHealth.maxHealth);

        Assert.Greater(PlayerHealth.damage, initialDamage, "Damage did not increase");
        //Debug.Log(initialDamage);
        //Debug.Log(PlayerHealth.damage);

        Assert.Greater(TopDownMovement.moveSpeed, initialMoveSpeed, "Speed did not increase");
        //Debug.Log(initialMoveSpeed);
        //Debug.Log(TopDownMovement.moveSpeed);

        Assert.Greater(PlayerHealth.regenAmount, initialRegenAmount, "Regen Amount did not increase");
        Debug.Log(initialRegenAmount);
        Debug.Log(PlayerHealth.regenAmount);

        Assert.Less(Shooting.firerate, initialFirerate, "Firerate did not decrease");
        //Debug.Log(initialFirerate);
        //Debug.Log(Shooting.firerate);
    }
}

