using UnityEngine;

public class RamenPickup : MonoBehaviour
{
    private Transform player;
    private bool isPickedUp = false;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPickedUp && !other.GetComponentInChildren<RamenPickup>())
        {
            player = other.transform;
            isPickedUp = true;
            GetComponent<Collider>().enabled = false; 
            transform.SetParent(player); 
            transform.localPosition = new Vector3(0, 1, 0);
            Debug.Log("🎒 Ramen picked up and attached to player!");
        }
    }
    void Update()
{
    if (player != null)
    {
        Debug.Log("👀 Player's child: " + player.GetComponentInChildren<RamenPickup>());
    }
}

    
}
