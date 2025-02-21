using UnityEngine;

public class RamenSpawner : MonoBehaviour
{
    public Transform[] spawnPoints; // Fixed spawn locations
    public GameObject ramenPrefab;
    private GameObject[] ramenInstances;
    private int currentRamenIndex = 0;

    void Start()
    {
        PreSpawnRamen();
        ActivateNextRamen();
    }

    void PreSpawnRamen()
    {
        ramenInstances = new GameObject[spawnPoints.Length];

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            ramenInstances[i] = Instantiate(ramenPrefab, spawnPoints[i].position, Quaternion.Euler(0, 0, 0));
            ramenInstances[i].SetActive(false); // Hide all ramen initially
        }
    }

    public void ActivateNextRamen()
    {
        if (currentRamenIndex < ramenInstances.Length)
        {
            ramenInstances[currentRamenIndex].SetActive(true); // Show next ramen
        }
        else
        {
            EndGame(); // No more ramen, end the game!
        }
    }

    public void RamenDelivered()
    {
        if (currentRamenIndex < ramenInstances.Length)
        {
            ramenInstances[currentRamenIndex].SetActive(false); // Hide the delivered ramen
            currentRamenIndex++;
            ActivateNextRamen(); // Activate the next ramen
        }
    }

    void EndGame()
    {
        Debug.Log("🎉 ALL RAMEN DELIVERED! GAME OVER!");
    }
}
