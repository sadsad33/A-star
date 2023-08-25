using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class PathFinding : MonoBehaviour
{
    AGrid grid;
    public Transform tracer, target;
    
    private void Awake()
    {
        grid = GetComponent<AGrid>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            FindPath(tracer.position, target.position);
        }
    }
    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        ANode startNode = grid.GetNodeFromWorldPoint(startPos);
        ANode targetNode = grid.GetNodeFromWorldPoint(targetPos);

        Heap<ANode> openList = new Heap<ANode>(grid.MaxSize);
        HashSet<ANode> closedList = new HashSet<ANode>();
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            ANode currentNode = openList.RemoveFirst();
            closedList.Add(currentNode);

            if (currentNode == targetNode)
            {
                sw.Stop();
                print("Path Found : " + sw.ElapsedMilliseconds + "ms"); 
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (ANode neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.isWalkable || closedList.Contains(neighbour)) continue;

                int newCurrentToNeighbourCost = currentNode.gCost + GetDistanceCost(currentNode, neighbour);
                if (newCurrentToNeighbourCost < neighbour.gCost || !openList.Contains(neighbour))
                {
                    neighbour.gCost = newCurrentToNeighbourCost;
                    neighbour.hCost = GetDistanceCost(neighbour, targetNode);
                    neighbour.parentNode = currentNode;

                    if (!openList.Contains(neighbour)) openList.Add(neighbour);
                }
            }
        }
    }

    void RetracePath(ANode startNode, ANode endNode)
    {
        List<ANode> path = new List<ANode>();
        ANode currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }
        path.Reverse();
        grid.path = path;
    }

    int GetDistanceCost(ANode nodeA, ANode nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY) return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }
}
