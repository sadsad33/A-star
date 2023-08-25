using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGrid : MonoBehaviour
{
    //public Transform playerPos;
    public bool onlyDisplayPathGizmos;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;

    ANode[,] grid;

    float nodeDiameter;
    int gridSizeX;
    int gridSizeY;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }
    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    void CreateGrid()
    {
        grid = new ANode[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new ANode(walkable, worldPoint, x, y);
            }
        }
    }

    public List<ANode> GetNeighbours(ANode node)
    {
        List<ANode> neighbours = new List<ANode>();

        // �ֺ� 8������ Ž���Ѵ�.
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }


    //���� ��ǥ�κ��� �׸��� ��ǥ�� ���Ѵ�.
    public ANode GetNodeFromWorldPoint(Vector3 worldPosition)
    {
        //������ǥ�� �߽� (0, 0) �� �׸��� ��ǥ�� �߽�(gridWorldSize.x / 2, gridWorldSize.y / 2) �� �����ֱ� ���� x��ǥ�� y��ǥ�� ���� ���Ѵ�.
        //���� �׸����� ũ��� ���� ������� ��ȯ�Ѵ�.
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;

        //x ��ǥ�� y��ǥ�� 0�� 1������ ������ �����.
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    public List<ANode> path;
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if (onlyDisplayPathGizmos)
        {
            if (path != null)
            {
                foreach (ANode n in path)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - .1f));
                }
            }
        }
        else
        {
            if (grid != null)
            {
                //ANode playerNode = GetNodeFromWorldPoint(playerPos.position);
                foreach (ANode n in grid)
                {
                    Gizmos.color = (n.isWalkable) ? Color.white : Color.red;
                    //if(playerNode == n) Gizmos.color = Color.cyan;
                    if (path != null && path.Contains(n)) Gizmos.color = Color.yellow;
                    Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - .1f));
                }
            }
        }
    }
}
