using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Grabber : MonoBehaviour
{
    // detect which hand
    public GameObject controller;
    public SteamVR_Action_Boolean grabAction = null;

    private SteamVR_Behaviour_Pose c_Pose = null;
    //private FixedJoint c_Joint = null;
    private Interactable currentInteractable = null;
    public List<Interactable> contactInteractables = new List<Interactable>();

    private GameObject currentInteractableParent = null;
    private bool pickup = false;
    private void Awake()
    {

        c_Pose = controller.GetComponent<SteamVR_Behaviour_Pose>();
        //c_Joint = controller.GetComponent<FixedJoint>();
    }

    // Update is called once per frame
    void Update()
    {

        //// Down
        if (grabAction.GetState(c_Pose.inputSource))
        {
            if (pickup == false)
            {
                pickup = true;
                PickUp();
            }
        }
        else
        {
            pickup = false;
            if (contactInteractables.Count != 0 && currentInteractableParent)
            {
                foreach (Interactable interactable in contactInteractables)
                {
                    interactable.transform.parent = currentInteractableParent.transform;
                    
                }
                currentInteractableParent = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!other.gameObject.CompareTag("Grabbable"))
        {

            return;
        }

        contactInteractables.Add(other.gameObject.GetComponent<Interactable>());
    }

    private void OnTriggerExit(Collider other)
    {

        if (!other.gameObject.CompareTag("Grabbable"))
        {

            return;
        }

        contactInteractables.Remove(other.gameObject.GetComponent<Interactable>());
    }

    public void PickUp()
    {

        // Get the closest interactable
        currentInteractable = GetClosestInteractable();

        // Null check
        if (!currentInteractable)
        {

            return;
        }

        //// Already held check
        //if (currentInteractable.activeHand)
        //{

        //    //currentInteractable.activeHand.Drop();
        //}

        // Position it
        //currentInteractable.transform.position = transform.position;
        //currentInteractable.transform.rotation = transform.rotation;

        if (currentInteractable.transform.parent.name != "Cone")
        {
            currentInteractableParent = currentInteractable.transform.parent.gameObject;
            currentInteractable.transform.parent = gameObject.transform;
        }
        // Attach to the controller
        //Rigidbody targetBody = currentInteractable.GetComponent<Rigidbody>();
        //c_Joint.connectedBody = targetBody;

        // Set active hand
        //currentInteractable.activeHand = this;

        //print("Hand Velocity"+ c_Pose.GetVelocity().sqrMagnitude);

        //print("Hand Angular Velocity" + c_Pose.GetAngularVelocity());
    }

    //public void Drop()
    //{

    //    // Null check
    //    if (!currentInteractable)
    //    {

    //        return;
    //    }

    //    // Velocity
    //    Rigidbody targetBody = currentInteractable.GetComponent<Rigidbody>();
    //    targetBody.velocity = c_Pose.GetVelocity();
    //    targetBody.angularVelocity = c_Pose.GetAngularVelocity();

    //    // Detach from the joint
    //    c_Joint.connectedBody = null;

    //    // Clear
    //    //currentInteractable.activeHand = null;
    //    currentInteractable = null;
    //}

    private Interactable GetClosestInteractable()
    {

        Interactable closest = null;

        float minDistance = float.MaxValue;
        float distance = 0.0f;

        foreach (Interactable interactObject in contactInteractables)
        {

            // the smallest sqrMagnitude is the closest
            distance = (interactObject.transform.position - transform.position).sqrMagnitude;

            if (distance < minDistance)
            {

                minDistance = distance;
                closest = interactObject;
            }
        }

        return closest;
    }
}
