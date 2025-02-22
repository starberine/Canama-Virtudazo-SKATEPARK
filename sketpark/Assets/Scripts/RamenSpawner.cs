using UnityEngine;

public class RamenSpawner : MonoBehaviour
{
    public Transform[] spawnPoints; 
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
            ramenInstances[i] = Instantiate(ramenPrefab, spawnPoints[i].position, ramenPrefab.transform.rotation);
            ramenInstances[i].SetActive(false); 
        }
    }

    public void ActivateNextRamen()
    {
        if (currentRamenIndex < ramenInstances.Length)
        {
            ramenInstances[currentRamenIndex].SetActive(true); 
        }
        else
        {
            EndGame(); 
        }
    }

    public void RamenDelivered()
    {
        if (currentRamenIndex < ramenInstances.Length)
        {
            ramenInstances[currentRamenIndex].SetActive(false); 
            currentRamenIndex++;
            ActivateNextRamen(); 
        }
    }

    void EndGame()
    {
        Debug.Log("🎉 ALL RAMEN DELIVERED! GAME OVER!");
    }
}
