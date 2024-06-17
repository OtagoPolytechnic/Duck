using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class playerMovementTest
{
    
    [UnityTest]
    public IEnumerator MovementTestWithEnumeratorPasses()
    {


        GameObject gameObject = new GameObject();
        Rigidbody2D rb2d = gameObject.AddComponent<Rigidbody2D>();
        //From Rohan: I had to change this chunk of code because I changed the moveSpeed var to Static, might cause a merge conflict
        TopDownMovement.moveSpeed = 5f; 

       
        rb2d.velocity = new Vector2(5f, 0f);
       
        yield return null;

        Assert.AreEqual(new Vector2(5f, 0f), rb2d.velocity);

       
        Object.DestroyImmediate(gameObject);
        
        
    }
}