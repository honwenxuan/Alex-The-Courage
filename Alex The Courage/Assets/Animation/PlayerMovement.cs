using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameManager gameManager;
    public Material blueOverlayMaterial; // Reference to the blue overlay material (assign this in the Inspector)

    private Renderer playerRenderer; // Reference to the player's mesh renderer
    private Material originalMaterial; // Store the player's original material
    private Color originalColor; // Store the player's original color


    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private float jumpButtonGracePeriod;
    [SerializeField]
    private float jumpHorizontalSpeed;
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private float walkingSpeed = 1.5f;

    private float dashSpeed = 10f; // units per second
    [SerializeField]
    private float dashDistance = 6f;
    [SerializeField]
    private float dashCooldown = 2f;

    private float dashDuration = 1f; // dash lasts for 0.5 seconds
    private float dashDistanceCovered = 0f; // track how much of the dash distance has been covered
    private float? lastDashTime;

    private Animator animator;
    private CharacterController characterController;
    private float ySpeed;
    private float originalStepOffset;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;
    private bool isJumping;
    private bool isGrounded;
    private bool isDashing;
    float startColliderHeight = 0;
    private float originalWalkingSpeed;

    public AudioSource footsteps;

    //knockback
    private Vector3 knockbackDirection;
    private float knockbackDuration = 0.5f;  // 0.5 seconds, change as needed
    private float knockbackSpeed = 15f;  // speed of knockback
    private float? knockbackStartTime;  // time when knockback started
    private bool isKnockedBack = false;  // flag for knockback state
    private float? lastKnockbackTime;
    private float knockbackCooldown = 1.0f;
    void Start()
    {
        // Find the player's mesh renderer in the child objects
        playerRenderer = GetComponentInChildren<Renderer>();

        // Store the original material and color
        originalMaterial = playerRenderer.material;
        originalColor = originalMaterial.color;

        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
        startColliderHeight = characterController.height;
        originalWalkingSpeed = walkingSpeed;
        //characterController.skinWidth = 0.02f; // Adjust the skin width as needed
    }

    void Update()
    {
        if (isKnockedBack)
        {
            float elapsed = Time.time - knockbackStartTime.Value;
            if (elapsed < knockbackDuration)
            {
                // Apply knockback movement
                Vector3 knockbackVelocity = knockbackDirection * knockbackSpeed;
                knockbackVelocity.y = ySpeed;
                characterController.Move(knockbackVelocity * Time.deltaTime);
                return;  // Exit the Update early if knockback is in effect
            }
            else
            {
                isKnockedBack = false;
            }
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // footstep sound
        if (isGrounded && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
        {
            footsteps.enabled = true;
        }
        else
        {
            footsteps.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.E) && inputMagnitude > 0.1f && !isJumping && !isDashing && isGrounded && (!lastDashTime.HasValue || Time.time - lastDashTime.Value > dashCooldown))
        {
            lastDashTime = Time.time;
            isDashing = true;
            dashDistanceCovered = 0f;
            animator.SetBool("IsDashing", isDashing);

            // roll sound
            FindObjectOfType<AudioManager>().Play("Roll");
        }

        if (isDashing)
        {
            float dashAmountThisFrame = dashSpeed * Time.deltaTime;
            if (dashDistanceCovered + dashAmountThisFrame > dashDistance)
            {
                dashAmountThisFrame = dashDistance - dashDistanceCovered;
            }

            Vector3 dashDirection = cameraTransform.forward * verticalInput + cameraTransform.right * horizontalInput;
            dashDirection.Normalize();
            dashDirection *= dashAmountThisFrame;
            dashDirection.y = ySpeed;

            characterController.Move(dashDirection);
            dashDistanceCovered += dashAmountThisFrame;

            if (Time.time - lastDashTime.Value > dashDuration)
            {
                isDashing = false;
                animator.SetBool("IsDashing", false);
                if (characterController.isGrounded)
                {
                    ySpeed = -5f; // Reset to some grounded value only when on the ground
                }

            }
            ySpeed = -5f;



            if (stateInfo.IsName("Dashing"))
            {
                float controllerHeight = animator.GetFloat("colliderheight");
                characterController.height = startColliderHeight * controllerHeight;
                float centerY = characterController.height / 2;
                characterController.center = new Vector3(characterController.center.x, centerY, characterController.center.z);
            }
        }
        else
        {
            characterController.height = startColliderHeight;
            float centerY = characterController.height / 2;
            characterController.center = new Vector3(characterController.center.x, centerY, characterController.center.z);
            ySpeed += Physics.gravity.y * Time.deltaTime;
        }

        animator.SetFloat("InputMagnitude", inputMagnitude, 0.05f, Time.deltaTime);
        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();
        ySpeed += Physics.gravity.y * Time.deltaTime;

        HandleGroundedStatus();
        HandleMovement(movementDirection, inputMagnitude);


    }
    private void Knockback(Vector3 directionFromCubeToPlayer)
    {
        Debug.Log("Knockback called");
        knockbackDirection = directionFromCubeToPlayer.normalized;
        knockbackStartTime = Time.time;
        isKnockedBack = true;
    }
    public void SlowDownPlayer(float slowDownFactor, float duration)
    {
        Debug.Log($"Original Speed: {originalWalkingSpeed}");

        // Change the material color to the blue overlay material
        playerRenderer.material = blueOverlayMaterial;

        // Slow down the player
        float newSpeed = originalWalkingSpeed * slowDownFactor;

        // Ensure the new speed does not fall below 1.0f
        if (newSpeed < 0.7f)
        {
            newSpeed = 0.7f;
        }

        walkingSpeed = newSpeed;

        Debug.Log($"New Speed: {walkingSpeed}");

        // Restore original speed after 'duration' seconds
        Invoke("RestoreSpeed", duration);
    }
    private void RestoreSpeed()
    {
        Debug.Log($"Restoring to Original Speed: {originalWalkingSpeed}");
        walkingSpeed = originalWalkingSpeed; // Restore the speed

        // Restore the original material and color
        playerRenderer.material = originalMaterial;
        playerRenderer.material.color = originalColor;
    }

    private void HandleGroundedStatus()
    {
        if (characterController.isGrounded)
        {
            lastGroundedTime = Time.time;
        }

        if (Input.GetButtonDown("Jump") && characterController.isGrounded) // Check if the player is grounded
        {
            jumpButtonPressedTime = Time.time;

            // jump sound
            FindObjectOfType<AudioManager>().Play("Jump");
        }

        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod && !isDashing)
        {
            // Ensure that stepOffset doesn't exceed the height
            characterController.stepOffset = Mathf.Min(originalStepOffset, characterController.height - 0.01f); // subtract a small value to ensure it's always slightly less than height

            ySpeed = -5f;
            animator.SetBool("IsGrounded", true);
            isGrounded = true;
            animator.SetBool("IsJumping", false);
            isJumping = false;
            animator.SetBool("IsFalling", false);

            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                ySpeed = jumpSpeed;
                animator.SetBool("IsJumping", true);
                isJumping = true;
                jumpButtonPressedTime = null;
                lastGroundedTime = null;
            }
        }
        else
        {
            characterController.stepOffset = 0;
            animator.SetBool("IsGrounded", false);
            isGrounded = false;

            if ((isJumping && ySpeed < 0) || ySpeed < -2)
            {
                animator.SetBool("IsFalling", true);
            }
        }
    }

    private void HandleMovement(Vector3 movementDirection, float inputMagnitude)
    {
        if (movementDirection != Vector3.zero && !isDashing)
        {
            animator.SetBool("IsMoving", true);
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }

        if (!isGrounded || isDashing)
        {
            Vector3 velocity = movementDirection * inputMagnitude * jumpHorizontalSpeed;
            velocity.y = ySpeed;
            characterController.Move(velocity * Time.deltaTime);
        }
    }

    private void OnAnimatorMove()
    {
        if (isGrounded && !isDashing)
        {
            Vector3 velocity = animator.deltaPosition * walkingSpeed;  // Multiply by walkingSpeed
            velocity.y = ySpeed * Time.deltaTime;
            characterController.Move(velocity);
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void HandleHitReaction()
    {

        // Trigger the 'GotHit' animation.
        animator.SetBool("IsHit", true);

        // Here you could add more reactions, like reducing health, etc.
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacless"))
        {
            if (!lastKnockbackTime.HasValue || Time.time - lastKnockbackTime.Value > knockbackCooldown)
            {
                // Calculate the direction from the obstacle to the player
                Vector3 directionFromCubeToPlayer = transform.position - hit.point;
                directionFromCubeToPlayer.y = 0; // Assuming you want to keep knockback horizontal
                directionFromCubeToPlayer.Normalize(); // Make it a unit vector

                Knockback(directionFromCubeToPlayer); // Call the Knockback function here

                lastKnockbackTime = Time.time;
            }
        }
        if (hit.gameObject.CompareTag("Enemy") || hit.gameObject.CompareTag("Bullet"))
        {
            HandleHitReaction();
            FindObjectOfType<AudioManager>().Play("Explode");
            FindObjectOfType<GameManager>().EndGame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacless"))
        {
            if (!lastKnockbackTime.HasValue || Time.time - lastKnockbackTime.Value > knockbackCooldown)
            {
                // Calculate the direction from the obstacle to the player
                Vector3 directionFromCubeToPlayer = transform.position - other.transform.position;
                directionFromCubeToPlayer.y = 0;  // Assuming you want to keep knockback horizontal
                directionFromCubeToPlayer.Normalize();  // Make it a unit vector

                Knockback(directionFromCubeToPlayer);  // Call the Knockback function here

                lastKnockbackTime = Time.time;

            }
        }

        if (other.gameObject.CompareTag("Checkpoint"))
        {
            gameManager.isCheckpointReached = true;
            gameManager.checkpoint = other.transform.position;
            Destroy(other.gameObject);
            FindObjectOfType<AudioManager>().Play("Checkpoint");
        }
    }



}
