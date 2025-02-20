using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; 
    public Vector3 offset = new Vector3(0, 2, -5); 
    public float smoothSpeed = 0.2f; 
    public float rotationSpeed = 3f; 

    private float yaw = 0f; 
    private float pitch = 15f; 

    private void Start()
    {
        if (player != null)
        {
            yaw = player.eulerAngles.y;
        }
        else
        {
            Debug.LogWarning("Player Transform is not assigned in the CameraFollow script!");
        }
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            
            if (Input.GetMouseButton(1)) 
            {
                yaw += Input.GetAxis("Mouse X") * rotationSpeed;
                pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
                pitch = Mathf.Clamp(pitch, -35f, 60f); 
            }
            else
            {
                
                yaw = Mathf.LerpAngle(yaw, player.eulerAngles.y, Time.deltaTime * rotationSpeed);
            }

            
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 targetPosition = player.position + rotation * offset;

            
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

            
            transform.LookAt(player.position + Vector3.up * 1.5f); 
        }
        else
        {
            Debug.LogWarning("Player Transform is not assigned in the CameraFollow script!");
        }
    }
}