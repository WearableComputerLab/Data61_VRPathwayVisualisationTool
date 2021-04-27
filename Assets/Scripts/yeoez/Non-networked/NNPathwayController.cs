/**
 * Controls the state of all node labels in the pathway.  
 * Author: Elyssa Yeo
 * Date: 5 Jan 2021
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NNPathwayController : MonoBehaviour
{
    public GameObject[] pathways;
    GameObject currentPathway;
    int currentPathwayIndex;

    private bool labelsActive;

    // TESTING ONLY
    public TextMesh text;

    private void Start()
    {
        // Show the first pathway only
        currentPathwayIndex = 0;
        currentPathway = pathways[currentPathwayIndex];
        for (int i = 1; i < pathways.Length; i++)
        {
            pathways[i].gameObject.SetActive(false);
        }


        labelsActive = false;
    }

    public void ToggleNodesLabel()
    {
        Selector[] pathwayNodes = currentPathway.GetComponentsInChildren<Selector>();
        foreach (var node in pathwayNodes)
        {
            if (!labelsActive)
            {
                node.NodeChild().ShowNodeText(true);
            }
            else
            {
                node.NodeChild().ShowNodeText(false);
            }
        }

        if (!labelsActive)
        {
            labelsActive = true;
        }
        else
        {
            labelsActive = false;
        }
    }
    public void ChangePathway(bool forward)
    {
        if (forward)
        {
            if (currentPathwayIndex == pathways.Length - 1)
            {
                currentPathwayIndex = 0;
            }
            else
            {
                currentPathwayIndex++;
            }
        }
        else
        {
            if (currentPathwayIndex == 0)
            {
                currentPathwayIndex = pathways.Length - 1;
            }
            else
            {
                currentPathwayIndex--;
            }
        }
        currentPathway.gameObject.SetActive(false);

        currentPathway = pathways[currentPathwayIndex];
        currentPathway.gameObject.SetActive(true);
    }
    public void ClearLog()
    {
        text.text = "";
    }
}
