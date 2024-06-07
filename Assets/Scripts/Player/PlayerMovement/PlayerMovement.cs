using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Input & Controllers
    CharacterController controller;
    InputActionAsset playerInput;
    Camera mainCamera;
    InputAction moveAction;
    InputAction jumpAction;

    [SerializeField] bool debugRays;
    Vector2 moveDirection;
    Vector3 playerVelocity;
    bool groundedPlayer;
    bool isJumping;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>().actions;
        mainCamera = Camera.main;
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
        jumpAction.canceled += ctx => Jump(false);

    }


    private void FixedUpdate()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        
        Vector3 inputDirection = new Vector3(moveDirection.x, 0.0f, moveDirection.y).normalized;

        Vector3 projection = Vector3.ProjectOnPlane(mainCamera.transform.forward, new Vector3(0, 1, 0)).normalized;

        Vector3 rightDirection = Vector3.Cross(projection, new Vector3(0, 1, 0)).normalized;


        
        Vector3 theFinalFinalMovement = projection * inputDirection.z + -rightDirection * inputDirection.x;


        if (debugRays)
        {
            Debug.DrawRay(controller.gameObject.transform.position, projection, Color.yellow);

            Debug.DrawRay(controller.gameObject.transform.position, rightDirection, Color.blue);

            Debug.DrawRay(controller.gameObject.transform.position, theFinalFinalMovement, Color.red);

        }


        controller.Move(theFinalFinalMovement * Time.deltaTime * playerSpeed);


        if (isJumping && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void SetMoveValue(Vector2 move)
    {
        moveDirection = move;
    }

    private void Jump(bool jumpState)
    {
        isJumping = jumpState;
    }

    //private void Move(Vector2 move)
    //{
    //    Debug.Log(move);
    //    float targetSpeed = moveSpeed;


    //    if(move == Vector2.zero)
    //    {
    //        targetSpeed = 0;
    //    }

    //    float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;
    //    currentHorizontalSpeed += acceleration * Time.deltaTime;

    //    if (currentHorizontalSpeed >= targetSpeed)
    //    {
    //        currentHorizontalSpeed = targetSpeed;
    //    }

    //    Vector3 inputDirection = new Vector3(move.x, 0.0f, move.y).normalized;

    //    if (move != Vector2.zero)
    //    {
    //        inputDirection = mainCamera.transform.right * move.x + mainCamera.transform.forward * move.y;

    //    }

    //    Debug.Log(currentHorizontalSpeed);
    //    Vector3 finalMove = new Vector3(inputDirection.x, -gravity * Time., inputDirection.z).normalized * (currentHorizontalSpeed * Time.deltaTime);

    //    controller.Move(finalMove);

    //}
}
