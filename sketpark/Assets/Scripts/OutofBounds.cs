using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var scoreManager = FindObjectOfType<ScoreManager>();
            if (scoreManager != null)
                scoreManager.LoseGame();
        }
    }
}
