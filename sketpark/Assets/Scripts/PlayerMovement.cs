using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6.0f;
    public float jumpHeight = 1.5f;
    public float gravity = -30.0f;
    public float slopeSpeedBoost = 1.5f;
    public float maxSlopeAngle = 45f;

    private CharacterController controller;
    public Vector3 velocity;
    private bool isGrounded;
    private bool canJump = true;
    private Animator animator;


    public Transform cameraTransform;
    public float rotationSpeed = 10.0f;
    public float tiltSpeed = 5f; 

    private Vector3 lastGroundNormal = Vector3.up;

   void Start()
{
    controller = GetComponent<CharacterController>();
    animator = GetComponentInChildren<Animator>(); // 🔥 FINDS THE ANIMATOR IN CHILD OBJECTS

    if (animator == null)
    {
        Debug.LogError("❌ Animator component NOT FOUND on the player or its children!");
    }
}


    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

         Vector3 moveDirection = cameraTransform.forward * moveZ + cameraTransform.right * moveX;
    moveDirection.y = 0;
    if (moveDirection.magnitude > 1) moveDirection.Normalize();

    animator.SetFloat("Speed", moveDirection.magnitude * speed); // 🔥 SET SPEED PARAMETER

    controller.Move(moveDirection * speed * Time.deltaTime);

        Vector3 groundNormal = Vector3.up; 
        if (OnSlope(out Vector3 slopeNormal))
        {
            groundNormal = slopeNormal;
            moveDirection = Vector3.ProjectOnPlane(moveDirection, slopeNormal) * slopeSpeedBoost;
        }
        lastGroundNormal = groundNormal; 

        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            Quaternion tiltRotation = Quaternion.FromToRotation(Vector3.up, lastGroundNormal) * targetRotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, tiltRotation, Time.deltaTime * tiltSpeed);
        }

        controller.Move(moveDirection * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded && canJump && moveDirection.magnitude > 0.1f)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            canJump = false;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.normal.y > 0.5f)
        {
            isGrounded = true;
            canJump = true;
            velocity.y = -2f; 
        }
    }

    bool OnSlope(out Vector3 slopeNormal)
    {
        slopeNormal = Vector3.up;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, controller.height / 2 + 0.5f))
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

    public void ApplyJumpBoost(float jumpForce)
    {
        if (controller.isGrounded)
        {
            velocity.y = jumpForce;
            controller.Move(new Vector3(0, velocity.y * Time.deltaTime, 0));
        }
    }
}
