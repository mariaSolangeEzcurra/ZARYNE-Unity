using UnityEngine;

<<<<<<< HEAD
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;        
    public float runSpeed = 8f;     
    public float jumpHeight = 1.5f;  
    public float gravity = -9.81f;  
=======

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float runSpeed = 8f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

>>>>>>> Geraldine

    private CharacterController controller;
    private Vector3 velocity;

<<<<<<< HEAD
=======

>>>>>>> Geraldine
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

<<<<<<< HEAD
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical");     

        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : speed;
        controller.Move(move * currentSpeed * Time.deltaTime);

=======

    void Update()
    {
        // --- Movimiento en plano ---
        float horizontal = Input.GetAxis("Horizontal"); // A/D
        float vertical = Input.GetAxis("Vertical");     // W/S


        Vector3 move = transform.right * horizontal + transform.forward * vertical;


        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : speed;
        controller.Move(move * currentSpeed * Time.deltaTime);


        // --- Salto ---
>>>>>>> Geraldine
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

<<<<<<< HEAD
=======

>>>>>>> Geraldine
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

<<<<<<< HEAD
=======

        // --- Gravedad ---
>>>>>>> Geraldine
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
