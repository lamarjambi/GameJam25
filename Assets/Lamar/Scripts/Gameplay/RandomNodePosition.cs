using UnityEngine;

public class RandomNodePosition : MonoBehaviour
{
    public Vector2 spawnAreaMin = new Vector2(-4f, -3f);
    public Vector2 spawnAreaMax = new Vector2(4f, 3f);

    void Awake()
    {
        Randomize();
    }

    public void Randomize()
    {
        transform.position = new Vector3(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y),
            0f
        );
    }
}
