using UnityEngine;
using System.Collections.Generic;

public class NodeManager : MonoBehaviour {
    // did this logic for my game Cosmic Thread game: https://github.com/lamarjambi/cosmic-thread
    // difference is, in CT, i drew 3D lines between them because the game is a canvas
    public static NodeManager Instance;

    private GameObject selectedNode = null;

    private List<(GameObject, GameObject, LineRenderer)> connections = new();

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ClearConnections();
    }

    void ClearConnections()
    {
        // press Esc to cleaar the connections/lines
        foreach (var (_, _, lr) in connections)
            Destroy(lr.gameObject);
        connections.Clear();
        selectedNode = null;
    }
    
    public void HandleNodeSelection(GameObject node) 
    {
        if (selectedNode == null) {
            selectedNode = node;
        } else if (selectedNode == node) {
            selectedNode = null;
        } else {
            CreateConnection(selectedNode, node);
            selectedNode = null;
        }
    }

    void CreateConnection(GameObject a, GameObject b) 
    {
        // checking for duplicates
        foreach (var (na, nb, _) in connections)
            if ((na == a && nb == b) || (na == b && nb == a)) return;

        GameObject lineObj = new GameObject("Connection");
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();

        lr.positionCount = 2;
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.material = new Material(Shader.Find("Sprites/Default"));

        bool correct = GameManager.Instance.IsCorrectConnection(a, b);

        // if it's correct connection, form the line as green, if not
        // form white ("default")
        lr.startColor = correct ? Color.green : Color.white;
        lr.endColor = correct ? Color.green : Color.white;
        lr.sortingLayerName = "Default";
        lr.sortingOrder = 1; 

        UpdateLinePosition(lr, a, b);
        connections.Add((a, b, lr));

        GameManager.Instance.CheckConnections();
    }

    public bool HasConnection(GameObject a, GameObject b)
    {
        foreach (var (na, nb, _) in connections)
            if ((na == a && nb == b) || (na == b && nb == a)) return true;
        return false;
    }

    void UpdateLinePosition(LineRenderer lr, GameObject a, GameObject b) 
    {
        lr.SetPosition(0, new Vector3(a.transform.position.x, a.transform.position.y, 0));
        lr.SetPosition(1, new Vector3(b.transform.position.x, b.transform.position.y, 0));
    }

    public void UpdateLines(GameObject movedNode) 
    {
        // so nodes move even if they're connected
        foreach (var (a, b, lr) in connections)
            if (a == movedNode || b == movedNode)
                UpdateLinePosition(lr, a, b);
    }
}