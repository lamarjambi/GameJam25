using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CorrectConnection
{
    public GameObject nodeA;
    public GameObject nodeB;
}

[Serializable]
public struct RoundConfig
{
    public List<CorrectConnection> correctConnections;
    public float timeLimit;
}

public class GameManager : MonoBehaviour
{
    // did the correct connections logic for my game Cosmic Thread game: https://github.com/lamarjambi/cosmic-thread
    public static GameManager Instance;

    [SerializeField] private List<RoundConfig> rounds = new();

    public int CurrentRound { get; private set; } = 0;

    // Kaylie can play with these
    public static event Action<int> OnRoundComplete; // 0, 1, 2
    public static event Action OnGameWon;
    public static event Action OnGameFailed;

    private float timer;
    private bool timerRunning = false;

    void Awake()
    {
        Instance = this;
        StartTimer();
    }

    void Update()
    {
        // failure when it hits zero
        if (!timerRunning) return;
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timerRunning = false;
            TriggerFailure();
        }
    }

    void StartTimer()
    {
        timer = GetCurrentTimeLimit();
        timerRunning = true;
    }

    public void RestartTimer() => StartTimer();

    public bool IsCorrectConnection(GameObject a, GameObject b)
    {
        if (CurrentRound >= rounds.Count) return false;
        foreach (CorrectConnection correct in rounds[CurrentRound].correctConnections)
            if ((correct.nodeA == a && correct.nodeB == b) || (correct.nodeA == b && correct.nodeB == a))
                return true;
        return false;
    }

    public void CheckConnections()
    {
        // :func: if all correct, round is over
        if (CurrentRound >= rounds.Count) return;
        var connections = rounds[CurrentRound].correctConnections;
        if (connections.Count == 0) return;

        // runs after every linkage
        foreach (CorrectConnection correct in connections)
        {
            if (!NodeManager.Instance.HasConnection(correct.nodeA, correct.nodeB))
                return;
        }

        OnAllConnectionsCorrect();
    }

    void OnAllConnectionsCorrect()
    {
        timerRunning = false;
        OnRoundComplete?.Invoke(CurrentRound);
        CurrentRound++;

        if (CurrentRound >= rounds.Count)
        {
            OnGameWon?.Invoke();
        }
        else
        {
            // delay
            Invoke(nameof(StartNextRound), 1.5f);
        }
    }

    void StartNextRound()
    {
        NodeManager.Instance.ResetRound();
        StartTimer();
    }

    public void TriggerFailure()
    {
        OnGameFailed?.Invoke();
    }

    public float GetCurrentTimeLimit()
    {
        if (CurrentRound >= rounds.Count) return 0f;
        return rounds[CurrentRound].timeLimit;
    }
}
