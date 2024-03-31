using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerTest
{
    [UnityTest]
    public IEnumerator PlayerTestWithEnumeratorPasses()
    {
        GameObject gameObject = new GameObject();
        Rigidbody2D rb2d = gameObject.AddComponent<Rigidbody2D>();
        TopDownMovement movementScript = gameObject.AddComponent<TopDownMovement>();

        movementScript.moveSpeed = 5f;

        Vector3 initialPosition = gameObject.transform.position;

        yield return null; // Wait for one frame to apply velocity

        // Simulate pressing the up arrow key (or equivalent)
        movementScript.moveInput = Vector2.up;

        yield return new WaitForSeconds(1f); // Wait for 1 second

        Assert.Greater(gameObject.transform.position.y, initialPosition.y); // Assert that the object has moved upwards
    }
}
