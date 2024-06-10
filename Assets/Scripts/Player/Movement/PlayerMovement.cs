using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Input & Controllers
    CharacterController controller;
    InputActionAsset playerInput;
    InputAction moveAction;
    InputAction jumpAction;
    Camera roomCamera;
    [SerializeField]
    Animator animator;

    [SerializeField] bool debugRays;
    [SerializeField] private float speedMultiplier = 1.0f;
    [SerializeField] private float airSpeed = 0.5f;
    [SerializeField] private float airAcceleration = 1.0f;
    [SerializeField] private float rotationSpeed = 1.5f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private Vector3 up = Vector3.up;

    Vector2 moveDirection;
    Vector3 playerVelocity;
    Vector3 currentVelocity;
    Vector3 lastDirection;
    Vector3 lastVelocity;
    bool jump;
    private bool lastGrounded = false;
    private bool isGrounded = false;
    private bool isJumping = false;
    private bool isFalling = false;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>().actions;
        roomCamera = Camera.main;
        moveAction = playerInput.FindAction("Move");
        jumpAction = playerInput.FindAction("Jump");
    }

    private void OnEnable()
    {
        moveAction.performed += ctx => SetMoveValue(ctx.ReadValue<Vector2>());
        moveAction.canceled += ctx => SetMoveValue(ctx.ReadValue<Vector2>());
        jumpAction.performed += ctx => Jump(true);
        jumpAction.canceled += ctx => Jump(false);
    }

    private void OnDisable() => Unsubscribe();

    private void OnDestroy() => Unsubscribe();

    private void Unsubscribe()
    {
        moveAction.performed -= ctx => SetMoveValue(ctx.ReadValue<Vector2>());
        moveAction.canceled -= ctx => SetMoveValue(ctx.ReadValue<Vector2>());
        jumpAction.performed -= ctx => Jump(true);
        jumpAction.canceled -= ctx => Jump(false);

    }

    private void Update()
    {
        if (roomCamera == null || !roomCamera.enabled)
        {
            return;
        }

        UpdateJump();

        Vector3 inputDirection = new Vector3(moveDirection.x, 0.0f, moveDirection.y).normalized;
        float inputMagnitude = Mathf.Clamp01(moveDirection.magnitude);
        // The projection of the camera's forward vector on the floor plane
        // gives a vector that is parallel to the floor no matter the rotation of the camera.
        Vector3 projection = Vector3.ProjectOnPlane(roomCamera.transform.forward, up).normalized;
        // If the projection is zero, it means the camera is looking at the up axis
        // in this case, the camera's up vector is parallel to the plane
        if (projection == Vector3.zero)
        {
            projection = roomCamera.transform.up;
        }
        Vector3 rightDirection = Vector3.Cross(projection, up).normalized;
        Vector3 movementDirection = projection * inputDirection.z + -rightDirection * inputDirection.x;

        if (debugRays)
        {
            Debug.DrawRay(controller.gameObject.transform.position, projection, Color.yellow);
            Debug.DrawRay(controller.gameObject.transform.position, rightDirection, Color.blue);
            Debug.DrawRay(controller.gameObject.transform.position, movementDirection, Color.red);
        }

        if (movementDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection, up);
            // Smoothly interpolate between the current rotation and the target rotation
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        animator.SetFloat("SpeedMultiplier", speedMultiplier);
        animator.SetFloat("Speed", inputMagnitude, 0.05f, Time.deltaTime);

        if (!isGrounded) {
            Vector3 velocity = movementDirection * airSpeed * inputMagnitude;
            /* Vector3 velocity = Vector3.Lerp(lastVelocity, movementDirection * airSpeed, airAcceleration * Time.deltaTime); */
            velocity.y = playerVelocity.y;
            controller.Move(velocity * Time.deltaTime);
        }
    }

    private void UpdateJump() {
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        if (isGrounded != lastGrounded)
        {
            lastGrounded = isGrounded;
            animator.SetBool("IsGrounded", isGrounded);

            if (isGrounded)
            {
                animator.SetBool("IsFalling", false);
                isJumping = false;
                isFalling = false;
            } else {
                if ((isJumping && playerVelocity.y > 0) || playerVelocity.y < -2)
                {
                    animator.SetBool("IsFalling", true);
                    isFalling = true;
                }
            }
        }

        if (jump && isGrounded)
        {
            animator.SetTrigger("Jump");
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            isJumping = true;
            jump = false;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
    }

    private void OnAnimatorMove() {
        if (isGrounded) {
            Vector3 velocity = animator.deltaPosition * 5; // model is scaled 5x
            velocity.y = playerVelocity.y * Time.deltaTime;
            controller.Move(velocity);
        }
    }

    private void SetMoveValue(Vector2 move)
    {
        moveDirection = move;
    }

    private void Jump(bool jump)
    {
        this.jump = jump;
    }

    public void SetCamera(Camera camera)
    {
        roomCamera = camera;
    }

    private void FaceDirection(Vector3 direction)
    {
        gameObject.transform.rotation = Quaternion.LookRotation(-direction, up);
    }
}
