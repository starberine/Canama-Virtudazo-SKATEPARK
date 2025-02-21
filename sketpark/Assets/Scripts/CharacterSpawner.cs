using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public GameObject[] characters; 
    public Transform spawnPoint; 
    public CameraFollow cameraFollow;

    void Start()
    {
        int selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        GameObject spawnedCharacter = Instantiate(characters[selectedCharacterIndex], spawnPoint.position, spawnPoint.rotation);

        // Assign the spawned character to CameraFollow
        if (cameraFollow != null)
        {
            cameraFollow.player = spawnedCharacter.transform;
        }

        // Assign cameraTransform to PlayerMovement
        PlayerMovement playerMovement = spawnedCharacter.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.cameraTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogError("PlayerMovement script is missing on the selected character prefab!");
        }
    }
}
