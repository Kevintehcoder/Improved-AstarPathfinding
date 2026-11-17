using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{
    Nodes[] allNodes;

    public static AStarPathfinding instance;

    public Nodes[] displayPath;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        allNodes = FindObjectsByType<Nodes>(FindObjectsSortMode.None);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (displayPath != null)
        {
            foreach (Nodes n in displayPath)
            {
                if (n.CameFrom == null)
                {
                    continue;
                }
                Gizmos.DrawLine(n.transform.position, n.CameFrom.transform.position); 

            }
        }
    }


    Nodes FindclosestNode(Transform focusNode)
    {
        float smallestDist = float.MaxValue;
        Nodes closestNode = null;

        foreach (Nodes node in allNodes)
        {
            float dist = Vector2.Distance(node.transform.position, focusNode.position);
            if (dist < smallestDist)
            {
                smallestDist = dist;
                closestNode = node;
            }
        }

        return closestNode;
    }

    public Nodes[] pathFind(Transform startPos, Transform endPos)
    {
        foreach (Nodes node in allNodes)
        {
            node.resetNode(endPos);
        }
        List<Nodes> possibleNodes = new List<Nodes>();
        Nodes currentNode = null;

        Nodes endNode = FindclosestNode(endPos); 

        Nodes startNode = FindclosestNode(startPos);
        startNode.setGscore(0);


        startNode.findFscore();
        currentNode = startNode;

        possibleNodes.Add(currentNode);

        while (possibleNodes.Count > 0)
        {
            int smallestFindex = 0;
            for (int i = 1; i < possibleNodes.Count; i++)
            {
                
                if (possibleNodes[i].fScore < possibleNodes[smallestFindex].fScore)
                {
                    smallestFindex = i;
                }
            }

            currentNode = possibleNodes[smallestFindex];
            possibleNodes.RemoveAt(smallestFindex);


            if (currentNode == endNode)
            {
                List<Nodes> path = new List<Nodes>();

                while (currentNode != null) 
                {
                    path.Add(currentNode);
                    currentNode = currentNode.CameFrom;
                }

                path.Reverse();
                displayPath = path.ToArray();
                return path.ToArray();
            }


            foreach (Nodes neighbor in currentNode.SurroundingNodes)
            {

                float heldGscore = neighbor.findGscore(currentNode.transform, currentNode.gscore);

                if (heldGscore < neighbor.gscore)
                {
                    neighbor.setGscore(heldGscore);
                    neighbor.Setorigin(currentNode);

                    neighbor.findFscore();
                    if (!possibleNodes.Contains(neighbor))
                    {
                        possibleNodes.Add(neighbor);
                    }

                }
                
            }
        }


        return null;
    }
}
