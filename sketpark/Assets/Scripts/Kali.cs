using UnityEngine;

public class Kali : SkateboardCharacter
{
    public float rotationSpeed = 8.0f;
    private bool canJump = true;

    void Awake()
    {
        characterName = "Kali";
        speed = 6.5f;
        jumpHeight = 1.8f;
        gravity = -28.0f;
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = Camera.main.transform.forward * moveZ + Camera.main.transform.right * moveX;
        moveDirection.y = 0;
        if (moveDirection.magnitude > 1) moveDirection.Normalize();

        animator.SetFloat("Speed", moveDirection.magnitude * speed);

        controller.Move(moveDirection * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded && canJump)
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
}
