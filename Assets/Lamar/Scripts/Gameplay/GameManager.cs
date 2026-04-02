using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public static bool IsLocked { get; private set; } = false;

    [SerializeField] private List<RoundConfig> rounds = new();

    public int CurrentRound { get; private set; } = 0;

    // Kaylie can play with these
    public static event Action<int> OnRoundComplete; // 0, 1, 2
    public static event Action OnGameWon;
    public static event Action OnGameFailed;

    private float timer;
    private bool timerRunning = false;
    
    private int resetCount = 0;


    [Header("Round Sequence")]
    private int roundID = 0;
    [SerializeField] private GameObject round1;
    [SerializeField] private GameObject round2;
    [SerializeField] private GameObject round3;

    void Awake()
    {
        Instance = this;
        IsLocked = false;
        StartTimer();
    }

    void Start()
    {
        round1.SetActive(false);
        round2.SetActive(false);
        round3.SetActive(false);
        RoundSequence();
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

        if (resetCount >= 3)
        {
            SceneManager.LoadScene("GameOver");
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
            Invoke(nameof(LoadWinScene), 1.5f);
        }
        else
        {
            // delay
            Invoke(nameof(StartNextRound), 1.5f);
        }
    }

    void LoadWinScene() => SceneManager.LoadScene("Win");

    void StartNextRound()
    {
        NodeManager.Instance.ResetRound();
        RoundSequence();
        StartTimer();
    }

    public void TriggerFailure()
    {
        IsLocked = true;
        OnGameFailed?.Invoke();

        resetCount++;
        Debug.Log("reset count: " + resetCount);
    }

    public static void Unlock()
    {
        IsLocked = false;
    }

    public float GetCurrentTimeLimit()
    {
        if (CurrentRound >= rounds.Count) return 0f;
        return rounds[CurrentRound].timeLimit;
    }

    public void RoundSequence()
    {   
        // nodes invisible by default
        switch (roundID)
        {
            case 0:
                round1.SetActive(true);
                roundID++;
                break;

            case 1:
                Destroy(round1);
                round2.SetActive(true);
                roundID++;
                break;

            case 2:
                Destroy(round2);
                round3.SetActive(true);
                break;
        }
    }
}
