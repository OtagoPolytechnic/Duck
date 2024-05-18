using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor.SceneManagement;

public class BulletShootTest
{
    [UnityTest]
    public IEnumerator BulletIsInstantiatedWhenMouseClicked()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/MainScene.unity");

        // Find the shooting script attached to an object in the scene
        Shooting shooter = Object.FindObjectOfType<Shooting>();
        Assert.IsNotNull(shooter, "Shooting script not found in the scene.");

        // Get initial count of bullets
        int initialBulletCount = GameObject.FindGameObjectsWithTag("Bullet").Length;

        // Simulate mouse click
        shooter.Shoot(); // Call the Shoot method directly from the shooter script

        // Wait for a frame to allow bullet instantiation
        yield return null;

        // Get the count of bullets after the click
        int finalBulletCount = GameObject.FindGameObjectsWithTag("Bullet").Length;

        // Assert that a new bullet instance is created
        Assert.Greater(finalBulletCount, initialBulletCount, "No bullet instantiated when mouse clicked.");
    }
}
