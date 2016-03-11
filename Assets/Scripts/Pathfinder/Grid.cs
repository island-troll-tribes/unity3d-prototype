using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

    public bool displayGridGizmos;
    public LayerMask unwalkableMask;
    public LayerMask unit;
    private Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    float nodeDiameter;
	int gridSizeX, gridSizeY;
	
	void Awake() {
        nodeDiameter = nodeRadius * 2;
		gridWorldSize.x = (GetComponentInParent<MeshRenderer>().bounds.size.x);
		gridWorldSize.y = (GetComponentInParent<MeshRenderer>().bounds.size.z);
		gridSizeY = Mathf.RoundToInt (gridWorldSize.x / nodeDiameter);
		gridSizeX = Mathf.RoundToInt (gridWorldSize.y / nodeDiameter);
		CreateGrid ();
	}
	
	public int MaxSize {
		get {
			return gridSizeX * gridSizeY;
		}
	}
	
	void CreateGrid() {
		grid = new Node[gridSizeX, gridSizeY];
		float marginX = gridWorldSize.x - gridSizeX*nodeDiameter;
		float marginY = gridWorldSize.y - gridSizeY*nodeDiameter;
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 + Vector3.right * marginX/2 - Vector3.forward * gridWorldSize.y / 2 + Vector3.forward * marginY/2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                Vector3 checkBox = new Vector3(nodeDiameter, nodeDiameter, nodeDiameter);  
                bool walkable = !(Physics.CheckBox(worldPoint, checkBox, Quaternion.identity, unwalkableMask ));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

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


    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if (grid != null && displayGridGizmos)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}