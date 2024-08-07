using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsCheckerScript : MonoBehaviour
{
    // Reference to the ItemController script
    private ItemController itemController;

    // Variables to store initial stats
    public float initialDamage;
    public float initialMaxHealth;
    public float initialMoveSpeed;
    public float initialRegenAmount;
    public float initialFirerate;
    public float initialBleedAmount;
    public float initialLifestealAmount;
    public float initialExplosiveBullets;
    public int initialEggCount;
    public float initialCritChance;
    public float initialMaxHelath;
    public float initialShotgun;

    void Start()
    {
        // Capture and log initial stats
        initialDamage = PlayerStats.Instance.Damage;
        initialMaxHealth = PlayerStats.Instance.MaxHealth;
        initialMoveSpeed = TopDownMovement.Instance.MoveSpeed;
        initialRegenAmount = PlayerStats.Instance.RegenAmount;
        initialFirerate = Shooting.Instance.Firerate;
        initialBleedAmount = EnemyHealth.bleedAmount;
        initialBleedAmount = PlayerStats.Instance.LifestealAmount;
        initialExplosiveBullets = PlayerStats.Instance.ExplosionSize;
        initialCritChance = PlayerStats.Instance.CritChance;
        initialMaxHelath = PlayerStats.Instance.MaxHealth;
        initialShotgun = PlayerStats.Instance.BulletAmount;

        Debug.Log($"Initial Damage: {initialDamage}");
        Debug.Log($"Initial Max health: {initialMaxHealth}");
        Debug.Log($"Initial Speed: {initialMoveSpeed}");
        Debug.Log($"Initial Regen amount: {initialRegenAmount}");
        Debug.Log($"Initial Firerate: {initialFirerate}");
        Debug.Log($"Initial BleedAmount: {initialBleedAmount}");
        Debug.Log($"Initial LifestealAmount: {initialBleedAmount}");
        Debug.Log($"Initial ExplosiveBullets: {initialExplosiveBullets}");
        Debug.Log($"Initial CritChance: {initialCritChance}");
        Debug.Log($"Initial MaxHelath: {initialMaxHelath}");
        Debug.Log($"Initial Shotgun: {initialShotgun}");

        // Find the ItemController component in the scene
        itemController = FindObjectOfType<ItemController>();

        if (itemController == null)
        {
            Debug.LogError("ItemController not found!");
        }
        else
        {
            initialEggCount = CountEggs();
            Debug.Log($"Initial Egg count: {initialEggCount}");

            // Start the coroutine to wait and then call the method
            StartCoroutine(CallItemPickedAfterDelay());
        }
    }
    private int CountEggs()
    {
        GameObject[] eggs = GameObject.FindGameObjectsWithTag("Egg");
        return eggs.Length;
    }
    // Coroutine to wait for 10 seconds and then call ItemPicked
    private IEnumerator CallItemPickedAfterDelay()
    {
        yield return new WaitForSeconds(10); // Wait for 10 seconds

        // Call ItemPicked methods
        itemController.ItemPicked(0);
        itemController.ItemPicked(01);
        itemController.ItemPicked(02);
        itemController.ItemPicked(03);
        itemController.ItemPicked(04);
        itemController.ItemPicked(05);
        itemController.ItemPicked(06);
        itemController.ItemPicked(07);
        itemController.ItemPicked(08);
        itemController.ItemPicked(09);
        itemController.ItemPicked(10);
        itemController.ItemPicked(11);
        itemController.ItemPicked(12);
        int updatedEggCount = CountEggs();
        Debug.Log($"Updated Egg count: {updatedEggCount}");
    }
}






