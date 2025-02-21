using UnityEngine;

public class RamenPickup : MonoBehaviour
{
    private Transform player;
    private bool isPickedUp = false;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPickedUp)
        {
            player = other.transform;
            isPickedUp = true;
            GetComponent<Collider>().enabled = false; // Disable collider after pickup
        }
    }

    void Update()
    {
        if (isPickedUp && player != null)
        {
            transform.position = Vector3.Lerp(transform.position, player.position + new Vector3(0, 1, 0), Time.deltaTime * 5f);
        }
    }
}
