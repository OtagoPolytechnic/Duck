using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MovementTest
{
    
    [UnityTest]
    public IEnumerator MovementTestWithEnumeratorPasses()
    {

        // Setup
        GameObject gameObject = new GameObject();
        Rigidbody2D rb2d = gameObject.AddComponent<Rigidbody2D>();
        TopDownMovement topDownMovement = gameObject.AddComponent<TopDownMovement>();
        topDownMovement.moveSpeed = 5f; // Assuming a speed

        // Simulate input
        float simulatedVerticalInput = 1.0f; // Simulate moving up
        topDownMovement.TestInput(new Vector2(0, simulatedVerticalInput)); // You need to create this method in TopDownMovement

        // Wait a frame for movement to apply
        yield return null;

        // Assert
        Assert.Greater(rb2d.velocity.y, 0);

        // Cleanup
        Object.Destroy(gameObject);
        
        
    }
}