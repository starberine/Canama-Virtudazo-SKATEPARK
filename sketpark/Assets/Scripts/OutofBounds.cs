using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    void Awake()
    {
        if (gameObject.tag != "OutofBounds")
        {
            Debug.LogWarning("⚠️ OutOfBounds script is attached to an object without the 'OutofBounds' tag!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.CompareTag("OutofBounds"))
        {
            var scoreManager = FindObjectOfType<ScoreManager>();
            if (scoreManager != null)
                scoreManager.LoseGame();
        }
    }
}
