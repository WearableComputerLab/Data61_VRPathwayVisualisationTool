/**
 * The player's hand on the network. Sets the hand to attach to the correct hand anchor.  
 * Author: Elyssa Yeo
 * Date: 5 Jan 2021
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkHand : MonoBehaviour
{
    public enum HandType
    {
        Left,
        Right
    }
    private Transform playerGlobal;
    private Transform playerLocal;
    private PhotonView photonView;
    public HandType handType;
    private void Start()
    {
        photonView = GetComponent<PhotonView>();

        if (photonView.IsMine)
        {
            playerGlobal = GameObject.Find("OVRPlayerController").transform;
            if (handType == HandType.Left)
            {
                playerLocal = playerGlobal.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor");
            }

            else if (handType == HandType.Right)
            {
                playerLocal = playerGlobal.Find("OVRCameraRig/TrackingSpace/RightHandAnchor");
            }

            this.transform.SetParent(playerLocal);
            this.transform.localPosition = Vector3.zero;
            this.transform.localRotation = Quaternion.identity;
        }
    }
}
