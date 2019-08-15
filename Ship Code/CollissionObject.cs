using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollissionObject : MonoBehaviour
{
    public ObjectHover engine;

    private void OnCollisionEnter(Collision other) {
        Debug.Log("boom");
        engine.On = false;   
    }
}
