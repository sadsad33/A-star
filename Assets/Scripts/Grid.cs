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

    // 현재 맵의 월드좌표를 중심으로 그리드를 만든다.
    // 방문할수 있는 노드와 방문 불가능한 노드를 여기서 가려낸다.
    void CreateGrid() {
        grid = new Node[gridSizeX, gridSizeY];
        // 월드 좌표의 좌측 최하단 좌표를 구한다. 해당 좌표는 그리드 상에서 (0,0)이 된다.
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

        // 주변 8방향을 탐색한다.
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

    // 맵의 월드 좌표가 음수라고 한다면,
    // 그리드의 한변의 길이보다 월드 좌표의 각 구성성분의 절대값이 월등히 크다면 어떻게 해야 할까??

    //월드 좌표로부터 그리드 좌표를 구한다.
    public Node GetNodeFromWorldPoint(Vector3 worldPosition) {
        //월드좌표의 중심 (0, 0) 을 그리드 좌표의 중심(gridWorldSize.x / 2, gridWorldSize.y / 2) 과 맞춰주기 위해 x좌표와 y좌표에 값을 더한다.
        //이후 그리드의 크기로 나눠 백분율로 변환한다.
        //float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        //float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;

        float percentX = (worldPosition.x + correctionX) / gridWorldSize.x;
        float percentY = (worldPosition.z + correctionY) / gridWorldSize.y;
        
        Debug.Log("백분율 : " + percentX + ", " + percentY);
        // x 좌표와 y좌표를 0과 1사이의 값으로 만든다.
        // 그리드 내의 좌표를 제외하고 전부 제외할 수 있다.
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        Debug.Log("0~1 : " + percentX + ", " + percentY);

        // 인덱스가 0부터 시작하므로 1을 빼줌
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        Debug.Log("그리드 x좌표 : " + x);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        Debug.Log("그리드 y좌표 : " + y);
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
