using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6.0f;
    public float jumpHeight = 1.5f;
    public float gravity = -30.0f;
    public float slopeSpeedBoost = 1.5f;
    public float maxSlopeAngle = 45f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool canJump = true;

    public Transform cameraTransform;
    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;

    public float rotationSpeed = 10.0f;
    public float tiltSpeed = 5f; 

    private Vector3 lastGroundNormal = Vector3.up;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            canJump = true;
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = cameraTransform.forward * moveZ + cameraTransform.right * moveX;
        moveDirection.y = 0;
        if (moveDirection.magnitude > 1) moveDirection.Normalize();

        // **🚀 NEW: FORCE TILT ON SLOPES**
        Vector3 groundNormal = Vector3.up; 
        if (OnSlope(out Vector3 slopeNormal))
        {
            groundNormal = slopeNormal;
            moveDirection = Vector3.ProjectOnPlane(moveDirection, slopeNormal); // 🔥 Moves correctly on slope
            moveDirection *= slopeSpeedBoost; 
        }
        lastGroundNormal = groundNormal; 

        // **🚀 FIXED: FULLY OVERRIDE ROTATION FOR TILT**
        if (moveDirection.magnitude > 0.1f)
        {
            // First, rotate towards movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            // THEN apply the tilt to match ground normal
            Quaternion tiltRotation = Quaternion.FromToRotation(Vector3.up, lastGroundNormal) * targetRotation;

            // 🚀 FINAL: Force both rotations
            transform.rotation = Quaternion.Slerp(transform.rotation, tiltRotation, Time.deltaTime * tiltSpeed);
        }

        controller.Move(moveDirection * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded && moveDirection.magnitude > 0.1f && canJump)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            canJump = false;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // **🚀 FINAL FIX: FORCE SLOPE DETECTION**
    bool OnSlope(out Vector3 slopeNormal)
    {
        slopeNormal = Vector3.up;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundDistance + 0.5f, groundMask))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (slopeAngle > 0 && slopeAngle < maxSlopeAngle)
            {
                slopeNormal = hit.normal;
                return true;
            }
        }
        return false;
    }
}
