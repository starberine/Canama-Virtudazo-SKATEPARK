using UnityEngine;
using System.Collections; // ✅ REQUIRED for coroutines

public class PlayerMovement : SkateboardCharacter
{
    public float slopeSpeedBoost = 1.5f;
    public float maxSlopeAngle = 45f;

    private CharacterController controller;
    private bool canJump = true;
    public Transform cameraTransform;
    public float rotationSpeed = 10.0f;
    public float tiltSpeed = 5f;

    private Vector3 lastGroundNormal = Vector3.up;

    // 🔥 Grinding Variables
    private bool isGrinding = false;
    private Transform rail;
    private Vector3 grindStartPos;
    private Vector3 grindEndPos;
    private float grindProgress = 0f;
    public float grindSpeed = 10f;
    private float railHeightOffset = 0.3f;
    private bool canGrind = true; // Prevent instant re-triggering

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // ✅ Check if Animator exists from SkateboardCharacter (Parent Class)
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>(); 
        }

        if (animator == null)
        {
            Debug.LogError("❌ Animator component NOT FOUND on the player or its children!");
        }
    }

    void Update()
{
    if (isGrinding)
    {
        HandleGrinding();
    }
    else
    {
        HandleMovement();
    }

    // ✅ FIXED: FORCE Unity to detect MidAir animation!
    bool isInAir = !controller.isGrounded; 
    animator.SetBool("MidAir", isInAir); // 🔥 NOW IT WORKS!!!

    // ✅ BACKFLIP STILL WORKS (even midair!)
    if (Input.GetKeyDown(KeyCode.F)) 
    {
        StartCoroutine(Backflip());
    }
}


    private void HandleMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = cameraTransform.forward * moveZ + cameraTransform.right * moveX;
        moveDirection.y = 0;
        if (moveDirection.magnitude > 1) moveDirection.Normalize();

        animator.SetFloat("Speed", moveDirection.magnitude * speed);

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
            animator.SetTrigger("JumpTrigger");
            canJump = false;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // ✅ Fixed Backflip Coroutine (No Errors!)
 IEnumerator Backflip()
{
    float flipForce = 50f; // 🔥 Adjust this for bigger/smaller flips
    float flipDuration = 1f; 
    float elapsedTime = 0.1f;

    Rigidbody rb = GetComponent<Rigidbody>(); // Get Rigidbody

    // 🔥 Play the Backflip Animation
    animator.SetTrigger("BackflipTrigger");

    // ✅ FIXED: Use input-based movement direction instead of controller velocity!
    Vector3 moveDirection = transform.forward * speed; // 🔥 Keeps player moving forward

    // 🔄 Apply torque for a realistic physics-based backflip
    rb.AddTorque(Vector3.right * flipForce, ForceMode.Impulse);

    while (elapsedTime < flipDuration)
    {
        // ✅ FIXED: Player keeps moving WHILE flipping!!!
        controller.Move(moveDirection * Time.deltaTime); 

        elapsedTime += Time.deltaTime;
        yield return null;
    }

    // ✅ Stop rotation after flip completes
    rb.angularVelocity = Vector3.zero; 

    // ✅ Ensure player lands upright
    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
}



    private void HandleGrinding()
    {
        if (rail == null) return;

        grindProgress += Time.deltaTime * grindSpeed;
        transform.position = Vector3.Lerp(grindStartPos, grindEndPos, grindProgress);

        if (grindProgress >= 1f)
        {
            ExitGrind();
        }
    }

    private void ExitGrind()
    {
        isGrinding = false;
        rail = null;
        canJump = true; // Re-enable jumping
        canGrind = false; // Prevent immediate re-trigger

        transform.position += Vector3.down * 0.5f; // Drop player slightly to land properly
        Invoke(nameof(ResetGrind), 0.5f);
    }

    private void ResetGrind()
    {
        canGrind = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RailStartEnd") && canGrind)
        {
            StartGrinding(other.transform);
        }
    }

    private void StartGrinding(Transform railTransform)
    {
        isGrinding = true;
        rail = railTransform;
        grindProgress = 0f;
        canJump = false;
        canGrind = false;

        Vector3 railDirection = rail.forward;
        if (Vector3.Dot(transform.forward, rail.forward) < 0) 
        {
            railDirection = -rail.forward;
        }

        grindStartPos = rail.position + Vector3.up * railHeightOffset;
        grindEndPos = grindStartPos + railDirection * 5f;

        transform.position = grindStartPos;
        transform.rotation = Quaternion.LookRotation(railDirection);
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
