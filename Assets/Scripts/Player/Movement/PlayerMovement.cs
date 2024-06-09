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
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float airSpeed = 0.5f;
    [SerializeField] private float acceleration = 2.0f;
    [SerializeField] private float rotationSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float jumpDelay = 1.02f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private Vector3 up = Vector3.up;

    Vector2 moveDirection;
    Vector3 playerVelocity;
    Vector3 currentVelocity;
    Vector3 lastDirection;
    bool groundedPlayer;
    bool isJumping;
    private float jumpTimer = 0.0f;

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
        jumpAction.performed += ctx => Jump();
    }

    private void OnDisable() => Unsubscribe();

    private void OnDestroy() => Unsubscribe();

    private void Unsubscribe()
    {
        moveAction.performed -= ctx => SetMoveValue(ctx.ReadValue<Vector2>());
        moveAction.canceled -= ctx => SetMoveValue(ctx.ReadValue<Vector2>());
        jumpAction.performed -= ctx => Jump();

    }


    private void FixedUpdate()
    {
        if (roomCamera == null || !roomCamera.enabled)
        {
            return;
        }

        UpdateJump();

        Vector3 inputDirection = new Vector3(moveDirection.x, 0.0f, moveDirection.y).normalized;
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
        Vector3 theFinalFinalMovement = projection * inputDirection.z + -rightDirection * inputDirection.x;

        float speed = controller.isGrounded ? playerSpeed : airSpeed;
        currentVelocity = Vector3.Lerp(currentVelocity, theFinalFinalMovement * speed, acceleration * Time.deltaTime);

        if (debugRays)
        {
            Debug.DrawRay(controller.gameObject.transform.position, projection, Color.yellow);
            Debug.DrawRay(controller.gameObject.transform.position, rightDirection, Color.blue);
            Debug.DrawRay(controller.gameObject.transform.position, theFinalFinalMovement, Color.red);
        }

        if (theFinalFinalMovement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(projection * inputDirection.x + rightDirection * inputDirection.z, up);
            // Smoothly interpolate between the current rotation and the target rotation
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }


        controller.Move(currentVelocity * Time.deltaTime);
        if (controller.isGrounded)
        {
            animator.SetFloat("Speed", currentVelocity.magnitude / playerSpeed);
        }
    }

    private void UpdateJump() {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            animator.SetBool("IsFalling", false);
            playerVelocity.y = 0f;
        }
        // Gravity needs to be applied first so the ground check is able to run.
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        playerVelocity.y = 0;
        animator.SetBool("IsGrounded", controller.isGrounded);
        
        if (isJumping && controller.isGrounded)
        {
            if(jumpTimer == 0.0f)
            {
                animator.SetBool("IsFalling", true);
                animator.SetTrigger("Jump");
            }
            jumpTimer += Time.deltaTime;
            
            if (jumpTimer > jumpDelay)
            {
                Debug.Log("Jumping");
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
                controller.Move(playerVelocity * Time.deltaTime);
                isJumping = false;
            }
        }


    }

    private void SetMoveValue(Vector2 move)
    {
        moveDirection = move;
    }

    private void Jump()
    {
        jumpTimer = 0.0f;
        isJumping = true;
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
