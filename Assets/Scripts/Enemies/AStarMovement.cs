using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarPathfinding : MonoBehaviour
{
    [SerializeField] private int detectionWidth = 10, detectionHeight = 10;
    
    [SerializeField] private float stopDistance = 0f;
    [SerializeField] private float speed = 2f;
    //[SerializeField] private GameObject gridVisualizer;
    private LayerMask obstacleLayer;
    private List<GameObject> visualizers = new List<GameObject>();
    private GameObject sirGluten;
    private Vector2 destPos;
    private Rigidbody2D rb;
    private float nodeSize = 1.0f;
    private float distanceToSirGluten;
    

    private Dictionary<Vector2, Node> grid = new Dictionary<Vector2, Node>();
    List<Node> path = new List<Node>();


    void Start()
    {
        sirGluten = GameObject.Find("SirGluten");
        rb = GetComponent<Rigidbody2D>();
        obstacleLayer = LayerMask.GetMask("Walls");
        
        StartCoroutine(StartPathfinding());
    }

    void Update() {
        distanceToSirGluten = Vector2.Distance(rb.position, sirGluten.transform.position);
        if (distanceToSirGluten <= stopDistance) {
            rb.linearVelocity = Vector2.zero;
            if (sirGluten.transform.position.x > rb.position.x) {
                transform.localScale = new Vector3(1f, 1f, 1f);
            } else {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            return;
        }

        if (distanceToSirGluten <= 2f) {
            rb.linearVelocity = Vector2.zero;
            rb.linearVelocity = ((Vector2)sirGluten.transform.position - rb.position).normalized * speed;
            return;
        }

        if (rb.linearVelocity.x < -1f) {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        } else if (rb.linearVelocity.x > 1f) {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }


        if (path.Count > 0) {
            if (Vector2.Distance(rb.position, path[path.Count - 1].Position) < 1f) {
                path.RemoveAt(path.Count - 1);
            }

            Vector2 movementDirection = (path[path.Count - 1].Position - rb.position);
            rb.linearVelocity = movementDirection.normalized * speed;

            bool reachedDistance = Vector2.Distance((Vector2)sirGluten.transform.position, rb.position) <= stopDistance;
            if (reachedDistance) {
                rb.linearVelocity = Vector2.zero;
            }

  
        }
    }

    IEnumerator StartPathfinding(){
        while(distanceToSirGluten >= 2f) yield return null;
        yield return new WaitForSeconds(1f);
        CreatePath();
        StartCoroutine(MoveTo());
    }

    IEnumerator MoveTo(){
        CreatePath();
        yield return new WaitForSeconds(0.1f);

        StartCoroutine(MoveTo());
        
    }
    
    Vector2 FindClosestNode(Vector2 targetPos) {
        float closestDistance = float.MaxValue;
        Node closestNode = null;

        foreach (var node in grid.Values) {
            float distance = Vector2.Distance(node.Position, targetPos);
            if (distance < closestDistance) {
                closestDistance = distance;
                closestNode = node;
            }
        }

        if (closestNode != null) {
            return closestNode.Position;
        }

        return Vector2.zero; 
    }


    void CreatePath() {
        GenerateGrid();
        foreach(GameObject obj in visualizers) Destroy(obj);

        Vector2 destPos = new Vector2(Mathf.RoundToInt(sirGluten.transform.position.x), Mathf.RoundToInt(sirGluten.transform.position.y));;
        if (!grid.ContainsKey(destPos)) {
            destPos = FindClosestNode(sirGluten.transform.position);
        }       

        Vector2 curPos = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        if (!grid.ContainsKey(curPos)) {
            curPos = FindClosestNode((Vector2)transform.position);
        }  
        List<Node> openList = new List<Node>(){grid[curPos]};
        openList[0].HCost = Mathf.RoundToInt(Vector2.Distance(destPos, curPos) * 10);
        Dictionary<Vector2,Node> closedList = new Dictionary<Vector2, Node>();

        Node foundPath = null;
        int k = 0;
        while(true) {
            if (closedList.ContainsKey(destPos)) 
            {
                foundPath = closedList[destPos];
                break;
            }
            if (openList.Count == 0) {
                foundPath = null;
                break;
            }

            openList.Sort((a, b) => 
            {
                if (a.FCost == b.FCost)
                {
                    return a.HCost.CompareTo(b.HCost);
                }
                return b.FCost.CompareTo(a.FCost);
            });

            Node lowestF = openList[openList.Count - 1];
            openList.RemoveAt(openList.Count - 1);
            closedList[lowestF.Position] = lowestF;

            List<Node> adjancentTiles = new List<Node>();
            for (int i = -1; i < 2; i++) {
                for (int j = -1; j < 2; j++) {
                    if (i == 0 && j==0) continue;
                    Vector2 newPos = new Vector2(lowestF.Position.x + i, lowestF.Position.y + j);
                    if (i!=0 && j!=0) {
                        if (!grid.ContainsKey(new Vector2(lowestF.Position.x + i, lowestF.Position.y)) && !grid.ContainsKey(new Vector2(lowestF.Position.x, lowestF.Position.y + j))) continue;
                    }

                    if (!grid.ContainsKey(newPos) || closedList.ContainsKey(newPos)) continue;


                    Node adjTile = new Node(newPos); 
                    adjTile.PrevNode = lowestF;
                    if (i != 0 && j != 0) {
                        adjTile.GCost = lowestF.GCost + 16;
                    } else {
                        adjTile.GCost = lowestF.GCost + 10;
                    }
                    adjTile.HCost = Mathf.RoundToInt(Vector2.Distance(destPos, newPos) * 10);
                    adjTile.FCost = adjTile.HCost + adjTile.GCost;

                    Node foundInOpen = null;
                    foreach (Node open in openList) if (open.Position == newPos) {
                        foundInOpen = open;
                    }

                    if (foundInOpen != null) {
                        if (adjTile.GCost > foundInOpen.GCost) {
                            foundInOpen.GCost = adjTile.GCost;
                            foundInOpen.FCost = adjTile.FCost; 
                            foundInOpen.PrevNode = lowestF;
                        }
                    } else {
                        openList.Add(adjTile);
                    }
                }
            }

            k++;
            if (k > 800) break;
        }

        Node curBacktrack = foundPath;
        path = new List<Node>();


        while (curBacktrack != null) {
            /*GameObject visualizer = Instantiate(gridVisualizer, curBacktrack.Position, Quaternion.identity);
            visualizers.Add(visualizer);
            visualizer.GetComponent<SpriteRenderer>().color = Color.green;
            visualizer.GetComponent<SpriteRenderer>().sortingOrder = 1;
            */

            path.Add(curBacktrack);
            curBacktrack = curBacktrack.PrevNode;
        }
    }

    void GenerateGrid()
    {
        grid = new Dictionary<Vector2, Node>();

        int startX = Mathf.RoundToInt(transform.position.x);
        int startY = Mathf.RoundToInt(transform.position.y);

        for (int y = startY - detectionHeight; y <= startY + detectionHeight; y++)
        {
            for (int x = startX - detectionWidth; x <= startX + detectionWidth; x++)
            {
                Vector2 nodePosition = new Vector2(x, y);

                if (Physics2D.OverlapCircle(nodePosition, nodeSize / 3, obstacleLayer) == null)
                {
                    grid[nodePosition] = new Node(nodePosition);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (grid == null) return;

        Gizmos.color = Color.cyan;

        foreach (var node in grid.Values)
        {
            Gizmos.DrawWireCube((Vector3)node.Position, Vector3.one * nodeSize);
        }
    }
}

public class Node
{
    private Vector2 position;
    private int fCost, gCost, hCost;
    private Node prevNode;

    public Node(Vector2 pos)
    {
        position = pos;

    }

    public Vector2 Position { get => position; set => position = value; }
    public global::System.Int32 FCost { get => fCost; set => fCost = value; }
    public global::System.Int32 GCost { get => gCost; set => gCost = value; }
    public global::System.Int32 HCost { get => hCost; set => hCost = value; }
    public Node PrevNode { get => prevNode; set => prevNode = value; }
}