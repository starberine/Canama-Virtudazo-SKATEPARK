using UnityEngine;
using System.Collections; 

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
    private bool isGrinding = false;
    private Transform rail;
    private Vector3 grindStartPos;
    private Vector3 grindEndPos;
    private float grindProgress = 0f;
    public float grindSpeed = 10f;
    private float railHeightOffset = 0.3f;
    private bool canGrind = true; 

    void Start()
    {
        controller = GetComponent<CharacterController>();

        
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
        isGrounded = controller.isGrounded;

        if (isGrinding)
        {
            HandleGrinding();
        }
        else
        {
            HandleMovement();
        }

        
        bool isInAir = !controller.isGrounded; 
        animator.SetBool("MidAir", isInAir); 

        
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

        if (Input.GetButtonDown("Jump") && isGrounded && canJump && moveDirection.magnitude > 0.1f)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetTrigger("JumpTrigger");
            canJump = false;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    
    IEnumerator Backflip()
    {
        float flipForce = 50f; 
        float flipDuration = 1f; 
        float elapsedTime = 0.1f;

        Rigidbody rb = GetComponent<Rigidbody>(); 
        animator.SetTrigger("BackflipTrigger");
        Vector3 moveDirection = transform.forward * speed; 
        rb.AddTorque(Vector3.right * flipForce, ForceMode.Impulse);

        while (elapsedTime < flipDuration)
        {       
            controller.Move(moveDirection * Time.deltaTime); 
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.angularVelocity = Vector3.zero; 
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
        canJump = true; 
        canGrind = false; 

        transform.position += Vector3.down * 0.5f; 
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
