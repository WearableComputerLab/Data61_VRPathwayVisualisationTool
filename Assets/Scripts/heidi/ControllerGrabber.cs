using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerGrabber : MonoBehaviour
{
    
    List<GameObject> objects = new List<GameObject>();

    public GameObject controller;
    public SteamVR_Action_Boolean grabAction = null;
    public SteamVR_Action_Boolean selectAction = null;
    private SteamVR_Behaviour_Pose c_Pose = null;

    private bool selectReleased;
    private GameObject grabbedObjectParent;
    private void Awake()
    {

        c_Pose = controller.GetComponent<SteamVR_Behaviour_Pose>();
        selectReleased = true;
    }

    public void OnTriggerEnter(Collider other) //picking up objects with rigidbodies
    {
        
        if (!other.gameObject.CompareTag("Grabbable"))
        {

            return;
        }

        objects.Add(other.gameObject);
    }

    public void OnTriggerExit(Collider other) // releasing those objects with rigidbodies
    {

        objects.Remove(other.gameObject);      
    }


    void Update() // refreshing program confirms trigger pressure and determines whether holding or releasing object

    {
        //float lTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
        //float rTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);

        //bool aButton = OVRInput.Get(OVRInput.Button.One); // pressed a button
        if (grabAction.GetState(c_Pose.inputSource))
        {
            Debug.Log("Grabbing.");
            foreach (GameObject go in objects)
            {

                GrabObject(go);
            }
        }
        else if (selectAction.GetState(c_Pose.inputSource) && selectReleased)
        {
            foreach (GameObject go in objects)
            {
                Selector selector = go.GetComponent<Selector>();
                if (selector != null)
                {
                    selector.Selected();
                }
            }
            selectReleased = false;
        }
        else if (!selectAction.GetState(c_Pose.inputSource) && !selectReleased)
        {
            selectReleased = true;
        }
        else
        {

            foreach (GameObject go in objects)
            {

                ReleaseObject(go);
            }
        }

        //if (aButton == true)
        //{
        //    Debug.Log("Button A is pressed.");
        //    // possibly showing the labels of the graph
        //    foreach (GameObject go in graphs)
        //    {
        //        ShowLables(go);
        //        //bool isGraph = CheckEnterGraphArea(go);

        //        //Debug.Log("is Graph: " + isGraph);

        //        //if (isGraph) {

        //        //    ShowLables(go);
        //        //}
             
        //    }
        //}
    }
    private void GrabObject(GameObject other) //create parentchild relationship between object and hand so object follows hand
    {

        Rigidbody rb = other.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        if (grabbedObjectParent == null)
        {
            grabbedObjectParent = other.transform.parent.gameObject;
        }
        other.transform.parent = gameObject.transform;
    }
    private void ReleaseObject(GameObject other) //removing parentchild relationship so you drop the object
    {

        Rigidbody rb = other.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = true;

        if (grabbedObjectParent)
        {
            other.transform.parent = grabbedObjectParent.transform;
            grabbedObjectParent = null;
        }
    }
}
