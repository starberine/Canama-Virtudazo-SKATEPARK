using UnityEngine;

public class SubmitRamen : MonoBehaviour
{
   void OnTriggerEnter(Collider other)
{
    Debug.Log("🚀 Something entered the SubmitRamen trigger: " + other.name);

    if (other.CompareTag("Player"))
    {
        Debug.Log("✅ Player detected inside SubmitRamen!");
        
        RamenPickup ramen = other.GetComponentInChildren<RamenPickup>();

        if (ramen != null)
        {
            Debug.Log("🍜 Ramen found! Submitting now...");
            
            ramen.transform.SetParent(null);
            Destroy(ramen.gameObject);
            Debug.Log("🔥 Ramen successfully submitted and destroyed!");

            var scoreManager = FindObjectOfType<ScoreManager>();
            if (scoreManager != null) 
                scoreManager.DeliverRamen();
            else 
                Debug.LogError("❌ ScoreManager not found!");

            var ramenSpawner = FindObjectOfType<RamenSpawner>();
            if (ramenSpawner != null) 
                ramenSpawner.RamenDelivered();
            else 
                Debug.LogError("❌ RamenSpawner not found!");
        }
        else
        {
            Debug.LogWarning("⚠️ No Ramen found on Player!");
        }
    }
}

}
