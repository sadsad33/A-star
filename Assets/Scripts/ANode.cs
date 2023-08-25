using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ANode : IHeapItem<ANode>
{
    public bool isWalkable;
    public Vector3 worldPos;
    public int gridX, gridY;
    public int gCost, hCost;
    public ANode parentNode;
    int heapIndex;

    // »ý¼ºÀÚ
    public ANode(bool _walkAble, Vector3 _worldPos, int _gridX, int _gridY) 
    {
        isWalkable = _walkAble;
        worldPos = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }
    
    public int fCost
    {
        get { return gCost + hCost; }
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(ANode nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
