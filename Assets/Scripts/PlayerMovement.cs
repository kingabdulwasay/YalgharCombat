using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 150f;
    public float jumpForce = 1f;
    public float gravity = -9.81f;

    public Transform cam;

    float xRotation = 0f;
    float yVelocity;
    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        Vector3 move = HandleMovement();
        HandleJumpAndGravity(ref move);

        controller.Move(move * Time.deltaTime);
        GetComponent<PlayerAnimation>().MovementAnimations();
    }


    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -30f, 20f);
        cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    public Vector3 HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        return (transform.right * x + transform.forward * z) * speed;
    }

    void HandleJumpAndGravity(ref Vector3 move)
    {
        if (controller.isGrounded)
        {
            if (yVelocity < 0)
                yVelocity = -2f;

            if (Input.GetButtonDown("Jump")){
                yVelocity = Mathf.Sqrt(jumpForce * -2f * gravity);
                GetComponent<PlayerAnimation>().HandleJump();

            }
        }

        yVelocity += gravity * Time.deltaTime;
        move.y = yVelocity;
    }

 
}
