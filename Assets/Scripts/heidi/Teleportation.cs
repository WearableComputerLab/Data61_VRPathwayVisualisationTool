using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Teleportation : MonoBehaviour
{
    public Transform cameraRigTransform;
    public Transform headTransform; // The camera rig's head

    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean teleportAction;

    public Vector3 teleportReticleOffset; // Offset from the floor for the reticle to avoid z-fighting

    public GameObject laserPrefab; // The laser prefab
    private GameObject laser; // A reference to the spawned laser
    private Transform laserTransform; // The transform component of the laser for ease of use
    private Vector3 hitPoint; // Point where the raycast hits


    public GameObject teleportReticlePrefab; // Stores a reference to the teleport reticle prefab.
    private GameObject reticle; // A reference to an instance of the reticle
    private Transform teleportReticleTransform; // Stores a reference to the teleport reticle transform for ease of use

    private bool shouldTeleport = false; // True if there's a valid teleport target

    public GameObject teleportArea = null;
    public Renderer meshRenderer;

    //public Material highlightMaterial;
    public Material greenMaterial;
    private Material blueMaterial;

    private bool validTeleport = false;

    void Start()
    {
        laser = Instantiate(laserPrefab);
        laser.SetActive(false);
      
        blueMaterial = laser.GetComponent<Renderer>().material;

        laserTransform = laser.transform;
        reticle = Instantiate(teleportReticlePrefab);
        teleportReticleTransform = reticle.transform;

        //if (teleportArea != null)
        //{

        meshRenderer = teleportArea.GetComponent<Renderer>();
        //    meshRenderer.enabled = false;
        //}

        
    }

    void Update()
    {
        if (meshRenderer != null)
        {            
            meshRenderer.enabled = false;
        }

        if (teleportAction.GetState(handType))
        {


            //if (teleportArea != null)
            //{ 
            //    Debug.Log("down" + meshRenderer);

            //    meshRenderer.enabled = true;
            //}

            if (meshRenderer != null)
            {
                //Debug.Log("show");
                meshRenderer.enabled = true;
            }

            RaycastHit hit;

            //Send out a raycast from the controller
            if (Physics.Raycast(controllerPose.transform.position, transform.forward, out hit, 100))
            {
                hitPoint = hit.point;

                ShowLaser(hit);

                //Show teleport reticle
                reticle.SetActive(true);
                teleportReticleTransform.position = hitPoint + teleportReticleOffset;

                var currCollide = hit.collider.gameObject;
                if (currCollide.tag == "Teleport")
                {

                    laser.GetComponent<Renderer>().material = greenMaterial;
                    reticle.GetComponent<Renderer>().material = greenMaterial;

                    shouldTeleport = true;

                    
                }
                else
                {
                    laser.GetComponent<Renderer>().material = blueMaterial;
                    reticle.GetComponent<Renderer>().material = blueMaterial;

                    shouldTeleport = false;
                }
            }
        }
        else // Touchpad not held down, hide laser & teleport reticle
        {

            //if (teleportArea != null)
            //{

            //    meshRenderer.enabled = false;
            //}

            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }

            laser.SetActive(false);
            reticle.SetActive(false);
        }

        //Debug.Log("teleportation: "+shouldTeleport);
        //Touchpad released this frame & valid teleport position found
        if (!teleportAction.GetState(handType) && shouldTeleport)
        {
            Teleport();
        }
    }

    private void Reset()
    {

        //shouldTeleport = false;
        validTeleport = false;
      
        if (teleportArea != null)
        {

            meshRenderer.enabled = false;
        }
    }

    private void ValidateTeleport()
    {

        //if (validTeleport)
        //{

        //    if (triggerAction.GetStateDown(handType))
        //    {

        //        shouldTeleport = true;
        //        validTeleport = false;
        //    }

        //    if (teleportAction.GetState(handType))
        //    {

        //        shouldTeleport = true;
        //        validTeleport = false;
        //    }
        //}

        //// Touchpad released this frame & valid teleport position found
        //if ((teleportAction.GetStateUp(handType) && shouldTeleport) || (triggerAction.GetStateDown(handType) && shouldTeleport))
        //{
        //    Teleport();
        //}

        if (validTeleport && teleportAction.GetState(handType))
        {

            Teleport();
        }
    }

    public void EnableTeleport()
    {

        //validTeleport = true;
        shouldTeleport = true;
    }

    //// Adjust the reticle size based on the object it point to
    //private void AdjustReticleSize()
    //{
    //    if (!currCollide)
    //    {

    //        return;
    //    }

    //    Vector3 oriReticle = new Vector3(0.5f, 0.5f, 0.5f);

    //    //GameObject parentObject = currCollide.transform.parent.gameObject;
    //    var parentObject = currCollide.transform.parent.transform.parent;

    //    var collideScale = parentObject.gameObject.transform.localScale;

    //    if (currCollide.tag == "GraphCube")
    //    {

    //        reticle.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    //    }
    //    else if (collideScale.x >= 0.8f)
    //    {

    //        reticle.gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
    //    }
    //    else if (collideScale.x >= 0.5f)
    //    {

    //        reticle.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    //    }
    //    else if (collideScale.x >= 0.25f)
    //    {

    //        reticle.gameObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
    //    }
    //    else if (collideScale.x >= 0.01f)
    //    {

    //        reticle.gameObject.transform.localScale = new Vector3(0.008f, 0.008f, 0.008f);
    //    }
    //    else
    //    {

    //        reticle.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    //    }

    //}

    //public void DisableLaser()
    //{

    //    meshRenderer.enabled = false;
    //    laser.SetActive(false);
    //    reticle.SetActive(false);
    //}


    private void ShowLaser(RaycastHit hit)
    {
        laser.SetActive(true); //Show the laser
                               // the laser set close to the hit point
        laserTransform.position = Vector3.Lerp(controllerPose.transform.position, hitPoint, .5f); // Move laser to the middle between the controller and the position the raycast hit
        laserTransform.LookAt(hitPoint); // Rotate laser facing the hit point
        laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y,
            hit.distance); // Scale laser so it fits exactly between the controller & the hit point
    }

    private void Teleport()
    {
        shouldTeleport = false; // Teleport in progress, no need to do it again until the next touchpad release
        reticle.SetActive(false); // Hide reticle
        Vector3 difference = cameraRigTransform.position - headTransform.position; // Calculate the difference between the center of the virtual room & the player's head
        difference.y = 0; // Don't change the final position's y position, it should always be equal to that of the hit point

        cameraRigTransform.position = hitPoint + difference;
    }
}
