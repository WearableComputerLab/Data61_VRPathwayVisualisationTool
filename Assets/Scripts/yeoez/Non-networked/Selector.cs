using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Selector : MonoBehaviour
{
    private bool childActive;
    private NodeComponents nodeChild;
    void Start()
    {
        // Load nodeComponents
        var nodeChildPrefab = Resources.Load("NodeChild");
        if (nodeChildPrefab == null)
        {
            throw new FileNotFoundException("...no file found - please check the configuration");
        }
        GameObject nodeChildGO = (GameObject) Instantiate(nodeChildPrefab);
        nodeChildGO.transform.parent = gameObject.transform;
        nodeChildGO.transform.localPosition = Vector3.zero;
        nodeChildGO.transform.localRotation = Quaternion.identity;
        nodeChild = nodeChildGO.GetComponent<NodeComponents>();
        
        nodeChild.ShowAll(false);
        nodeChild.SetNodeText(GetComponent<DataPoint>().ID());
        nodeChild.SetImage("ChemicalStructures/" + GetComponent<DataPoint>().ID());
        nodeChild.SetScatterplot("ChemicalStructures/" + GetComponent<DataPoint>().ID() + "_scatterplot");

        Sprite loadedScatterplot = Resources.Load<Sprite>("ChemicalStructures/" + GetComponent<DataPoint>().ID() + "_scatterplot");
        if (!loadedScatterplot)
        {
            MeshRenderer mr = this.GetComponent<MeshRenderer>();
            Color col = mr.material.color;
            col.a = 120 / 255f; // pass float value here
            mr.material.color = col;
        }

        childActive = false;
    }

    public void Selected()
    {
        if (childActive)
        {
            childActive = false;
        } else
        {
            childActive = true;
        }
        nodeChild.ShowAll(childActive);
    }

    public NodeComponents NodeChild()
    {
        return nodeChild;
    }
}
