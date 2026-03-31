using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CorrectConnection
{
    public GameObject nodeA;
    public GameObject nodeB;
}

public class GameManager : MonoBehaviour
{
    // did this logic for my game Cosmic Thread game: https://github.com/lamarjambi/cosmic-thread
    public static GameManager Instance;

    [SerializeField] private List<CorrectConnection> correctConnections = new();

    void Awake()
    {
        Instance = this;
    }

    public bool IsCorrectConnection(GameObject a, GameObject b)
    {
        // cross-check if connections between the nodes are correct
        foreach (CorrectConnection correct in correctConnections)
            if ((correct.nodeA == a && correct.nodeB == b) || (correct.nodeA == b && correct.nodeB == a))
                return true;
        return false;
    }

    public void CheckConnections()
    {
        if (correctConnections.Count == 0) return;

        foreach (CorrectConnection correct in correctConnections)
        {
            if (!NodeManager.Instance.HasConnection(correct.nodeA, correct.nodeB))
                return;
        }

        OnAllConnectionsCorrect();
    }

    void OnAllConnectionsCorrect()
    {
        Debug.Log("all connections correct");
    }
}
