using UnityEngine;

public class JumpBoost : MonoBehaviour
{
    public float jumpForce = 15f;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("JumpBoost Triggered: " + other.name);

        if (!other.CompareTag("Player")) 
        {
            Debug.LogWarning("Non-player object entered: " + other.name);
            return;
        }

        PlayerMovement player = other.GetComponent<PlayerMovement>();

        if (player == null)
        {
            Debug.LogError("PlayerMovement script NOT found on player!");
            return;
        }

        Debug.Log("Boosting Player Jump! Jump Force: " + jumpForce);
        player.ApplyJumpBoost(jumpForce);
    }
}
