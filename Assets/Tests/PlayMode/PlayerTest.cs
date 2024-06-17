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

        movementScript.rb2d = rb2d; // Assuming you have a public field for Rigidbody2D in your TopDownMovement script
        TopDownMovement.moveSpeed = 5f;

        Vector3 initialPosition = gameObject.transform.position;

        yield return null; // Wait for one frame to apply velocity

        // Simulate pressing the up arrow key (or equivalent)
        rb2d.AddForce(Vector2.up * TopDownMovement.moveSpeed, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1f); // Wait for 1 second

        Assert.Greater(gameObject.transform.position.y, initialPosition.y); // Assert that the object has moved upwards
    }
}
