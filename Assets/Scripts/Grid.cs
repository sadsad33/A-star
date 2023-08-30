using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    //public Transform playerPos;
    public bool displayGridGizmos;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;

    Node[,] grid;

    float nodeDiameter, correctionX, correctionY;
    int gridSizeX;
    int gridSizeY;

    void Awake() {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        correctionX = gridSizeX/2 - transform.position.x;
        correctionY = gridSizeY/2 - transform.position.z;
        Debug.Log("correctionX : " + correctionX);
        Debug.Log("correctionY : " + correctionY);
        CreateGrid();
    }
    public int MaxSize {
        get { return gridSizeX * gridSizeY; }
    }

    // ���� ���� ������ǥ�� �߽����� �׸��带 �����.
    // �湮�Ҽ� �ִ� ���� �湮 �Ұ����� ��带 ���⼭ ��������.
    void CreateGrid() {
        grid = new Node[gridSizeX, gridSizeY];
        // ���� ��ǥ�� ���� ���ϴ� ��ǥ�� ���Ѵ�. �ش� ��ǥ�� �׸��� �󿡼� (0,0)�� �ȴ�.
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        for (int x = 0; x < gridSizeX; x++) {
            for (int y = 0; y < gridSizeY; y++) {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }
    
    public List<Node> GetNeighbours(Node node) {
        List<Node> neighbours = new List<Node>();

        // �ֺ� 8������ Ž���Ѵ�.
        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                if (x == 0 && y == 0) continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    neighbours.Add(grid[checkX, checkY]);
            }
        }
        return neighbours;
    }

    // ���� ���� ��ǥ�� ������� �Ѵٸ�,
    // �׸����� �Ѻ��� ���̺��� ���� ��ǥ�� �� ���������� ���밪�� ������ ũ�ٸ� ��� �ؾ� �ұ�??

    //���� ��ǥ�κ��� �׸��� ��ǥ�� ���Ѵ�.
    public Node GetNodeFromWorldPoint(Vector3 worldPosition) {
        //������ǥ�� �߽� (0, 0) �� �׸��� ��ǥ�� �߽�(gridWorldSize.x / 2, gridWorldSize.y / 2) �� �����ֱ� ���� x��ǥ�� y��ǥ�� ���� ���Ѵ�.
        //���� �׸����� ũ��� ���� ������� ��ȯ�Ѵ�.
        //float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        //float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;

        float percentX = (worldPosition.x + correctionX) / gridWorldSize.x;
        float percentY = (worldPosition.z + correctionY) / gridWorldSize.y;
        
        Debug.Log("����� : " + percentX + ", " + percentY);
        // x ��ǥ�� y��ǥ�� 0�� 1������ ������ �����.
        // �׸��� ���� ��ǥ�� �����ϰ� ���� ������ �� �ִ�.
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        Debug.Log("0~1 : " + percentX + ", " + percentY);

        // �ε����� 0���� �����ϹǷ� 1�� ����
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        Debug.Log("�׸��� x��ǥ : " + x);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        Debug.Log("�׸��� y��ǥ : " + y);
        return grid[x, y];
    }
  
    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null && displayGridGizmos) {
            foreach (Node n in grid) {
                Gizmos.color = (n.isWalkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}
