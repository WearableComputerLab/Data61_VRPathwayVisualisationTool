using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Interactable : MonoBehaviour
{

    [HideInInspector]
    public Grabber activeHand = null;

    public int touchCount;

    void start()
    {
        if (gameObject.tag != "Grabbable")
        {
            Debug.LogError("Interactable's tag is not set to grabbable");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        touchCount++;
    }
    private void OnCollisionExit(Collision collision)
    {
        touchCount--;
    }
}
