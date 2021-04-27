using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerSelector : MonoBehaviour
{

    List<GameObject> objects = new List<GameObject>();

    public GameObject controller;
    public SteamVR_Action_Boolean selectAction = null;
    private SteamVR_Behaviour_Pose c_Pose = null;
    private bool selectReleased;
    private void Awake()
    {
        c_Pose = controller.GetComponent<SteamVR_Behaviour_Pose>();
        selectReleased = true;
    }

    public void OnTriggerEnter(Collider other)
    {

        if (!other.gameObject.CompareTag("Grabbable"))
        {

            return;
        }

        objects.Add(other.gameObject);
    }

    public void OnTriggerExit(Collider other) 
    {

        objects.Remove(other.gameObject);
    }


    void Update()
    {
        if (selectAction.GetState(c_Pose.inputSource) && selectReleased)
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
    }

}
