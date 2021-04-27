    /**
 * Information components related to a metabolite. 
 * Author: Elyssa Yeo
 * Date: 5 Jan 2021
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NodeComponents : MonoBehaviour
{
    public TextMesh nodeLabel;
    public GameObject image;
    public GameObject scatterplot;

    private GameObject hmdCamera;
    private Vector3 hmdCameraPos;
    [PunRPC]
    public void ShowAll(bool active)
    {
        ShowNodeText(active);
        ShowImage(active);
        if (scatterplot)
        {
            ShowScatterplot(active);
            
        }
    }

    [PunRPC]
    public void SetNodeText(string text)
    {
        nodeLabel.text = text;         
    }

    [PunRPC]
    public void ShowNodeText(bool active)
    {
        nodeLabel.transform.parent.gameObject.SetActive(active);
        OrientateToCamera(nodeLabel.transform.parent.gameObject);
    }

    [PunRPC]
    public void SetImage(string path)
    {
        Sprite loadedImage = Resources.Load<Sprite>(path);
        if (!loadedImage) {

            loadedImage = Resources.Load<Sprite>("ChemicalStructures/NoData");
        }
        SpriteRenderer spriteRenderer = image.GetComponentInChildren<SpriteRenderer>();        
        spriteRenderer.sprite = loadedImage;
    }

    [PunRPC]
    public void SetScatterplot(string path)
    {
        Sprite loadedImage = Resources.Load<Sprite>(path);
        if (!loadedImage)
        {
            ShowScatterplot(false);
            scatterplot = null;
        }
        else
        {
            SpriteRenderer spriteRenderer = scatterplot.GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.sprite = loadedImage;
        }
    }

    [PunRPC]
    public void ShowImage(bool active)
    {
        image.SetActive(active);
        if (active)
        {
            OrientateToCamera(image);
        }
    }

    [PunRPC]
    public void ShowScatterplot(bool active)
    {
        scatterplot.SetActive(active);
        if (active)
        {
            OrientateToCamera(scatterplot);
        }
    }
    
    private void OrientateToCamera(GameObject go)
    {
        Vector3 v = transform.position - hmdCameraPos;
        Quaternion q = Quaternion.LookRotation(v);
        go.transform.rotation = q;
    }

    private void Update()
    {
        hmdCamera = GameObject.Find("/[CameraRig]/Camera (eye)");
        if (hmdCamera)
        {
            hmdCameraPos = hmdCamera.transform.position;
        }
    }
}
