using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 150f;
    public float jumpForce = 2.5f;
    public float gravity = -9.81f;

    public Transform cam;

    float xRotation = 0f;
    float yVelocity;
    CharacterController controller;
    public bool isGrounded;

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
        isGrounded = controller.isGrounded;
      
    }



    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -30f, 30f);
        cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    public Vector3 HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        float currentSpeed = speed;
        if (GetComponent<PlayerCombat>().isCrouching)
        {
            currentSpeed = speed * 0.5f; 
        }

        return (transform.right * x + transform.forward * z) * currentSpeed;
    }

    void HandleJumpAndGravity(ref Vector3 move)
    {
        if (controller.isGrounded)
        {
            if (yVelocity < 0)
                yVelocity = -2f;

            if (Input.GetButtonDown("Jump")){
                yVelocity = Mathf.Sqrt(jumpForce * -3f * gravity);
                GetComponent<PlayerAnimation>().HandleJump();

            }
        }

        yVelocity += gravity * Time.deltaTime;
        move.y = yVelocity;
    }


 
}
