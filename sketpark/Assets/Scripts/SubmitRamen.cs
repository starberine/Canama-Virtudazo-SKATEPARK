using UnityEngine;

public class SubmitRamen : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RamenPickup ramen = other.GetComponentInChildren<RamenPickup>();

            if (ramen != null)
            {
                ramen.transform.SetParent(null);
                Destroy(ramen.gameObject);

                var scoreManager = FindObjectOfType<ScoreManager>();
                if (scoreManager != null) 
                    scoreManager.DeliverRamen();

                var ramenSpawner = FindObjectOfType<RamenSpawner>();
                if (ramenSpawner != null) 
                    ramenSpawner.RamenDelivered();
            }
        }
    }
}
