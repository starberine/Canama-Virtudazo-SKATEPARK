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
                Destroy(ramen.gameObject); // Remove ramen
                FindObjectOfType<ScoreManager>().DeliverRamen(); // Increase score
            }
        }
    }
}
