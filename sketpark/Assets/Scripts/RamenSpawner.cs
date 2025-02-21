using UnityEngine;

public class RamenSpawner : MonoBehaviour
{
    public Transform[] spawnPoints; // Set in Inspector
    public GameObject ramenPrefab;
    private int currentRamenIndex = 0;
    private GameObject currentRamen;

    public void SpawnNextRamen()
    {
        if (currentRamen != null) Destroy(currentRamen); // Remove old ramen

        if (currentRamenIndex < spawnPoints.Length)
        {
            currentRamen = Instantiate(ramenPrefab, spawnPoints[currentRamenIndex].position, Quaternion.identity);
            currentRamenIndex++;
        }
    }
}
