using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailureEffect : MonoBehaviour
{
    [SerializeField] private GameObject joshPrefab;
    [SerializeField] private int spawnCount = 8;
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 3f;
    [SerializeField] private float timeBetweenSpawns = 0.2f;
    [SerializeField] private float displayDuration = 2f;

    private List<GameObject> spawnedJoshes = new();

    void OnEnable() => GameManager.OnGameFailed += OnFailed;
    void OnDisable() => GameManager.OnGameFailed -= OnFailed;

    void OnFailed() => StartCoroutine(SpawnSequence());

    IEnumerator SpawnSequence()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 screenPos = new Vector3(
                Random.Range(0f, Screen.width),
                Random.Range(0f, Screen.height),
                10f
            );
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            worldPos.z = 0f;

            GameObject josh = Instantiate(joshPrefab, worldPos, Quaternion.identity);
            josh.transform.localScale = Vector3.one * Random.Range(minScale, maxScale);
            spawnedJoshes.Add(josh);

            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        yield return new WaitForSeconds(displayDuration);
        ClearAndReset();
    }

    void ClearAndReset()
    {
        foreach (var josh in spawnedJoshes)
            Destroy(josh);
        spawnedJoshes.Clear();

        NodeManager.Instance.ResetRound();
        GameManager.Instance.RestartTimer();
    }
}
