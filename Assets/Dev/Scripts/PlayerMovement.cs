using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{

    [Header("References")]
    public Transform cameraTransform;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.6f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 0.08f; // tuned for Mouse Delta (new system)
    public float maxLookAngle = 90f;

    private CharacterController controller;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool jumpPressed;

    private Vector3 velocity;
    private float xRotation;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        jumpPressed = false; // consume jump
    }

    void HandleMovement()
    {
        Vector3 move = (transform.right * moveInput.x + transform.forward * moveInput.y);
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0f)
            velocity.y = -2f;

        if (jumpPressed && controller.isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        // Mouse delta comes in "pixels per frame-ish", so we scale softly
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    // PlayerInput (Send Messages) will call these automatically if the action names match
    public void OnMove(InputValue value) => moveInput = value.Get<Vector2>();
    public void OnLook(InputValue value) => lookInput = value.Get<Vector2>();
    public void OnJump(InputValue value)
    {
        if (value.isPressed) jumpPressed = true;
    }
}
