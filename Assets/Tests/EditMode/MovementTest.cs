using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MovementTest
{
    
    [UnityTest]
    public IEnumerator MovementTestWithEnumeratorPasses()
    {

        GameObject gameobject = new GameObject();
        
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
