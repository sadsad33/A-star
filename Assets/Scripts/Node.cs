using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//길찾기에 사용할 노드클래스
public class Node : IHeapItem<Node> {
    public bool isWalkable;
    public Vector3 worldPos;
    public int gridX, gridY;
    public int movementPenalty;
    public int gCost, hCost;
    public Node parentNode;
    int heapIndex;

    public Node(bool _isWalkable, Vector3 _worldPos, int _gridX, int _gridY, int _penalty){
        isWalkable = _isWalkable;
        worldPos = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
        movementPenalty = _penalty; 
    }

    public int fCost{
        get { return gCost + hCost; }
    }

    public int HeapIndex {
        get { return heapIndex; }
        set { heapIndex = value; }
    }


    /* 대상 값.CompareTo(비교 값) 의 결과
     * 대상 값 < 비교 값의 경우 -1
     * 대상 값 == 비교 값의 경우 0
     * 대상 값 > 비교 값의 경우 1
     */
    public int CompareTo(Node nodeToCompare){
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
            compare = hCost.CompareTo(nodeToCompare.hCost);
        return -compare;
    }
}
