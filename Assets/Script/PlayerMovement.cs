using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControl controls; // llamada archivo de InputSystem generado por PlayerControl.inputactions
    private Vector2 moveInput; // llamada valor del movimiento leído del InputSystem (WASD / Joystick)

    private Animator animator; // llamada valor de rotacion leído del InputSystem (AD / Joystick)
    private Rigidbody rb;

    [SerializeField]  private float rotateSpeed; // velocidad rotación (grados por segundo)
    [SerializeField]  private float walkSpeed; // velocidad al caminar 
    [SerializeField]  private float runSpeed; // velocidad al correr 

    private bool isRunning;

    private void Awake()
    {   
        controls = new PlayerControl(); // inicia el sistema de controles
        
        controls.MovePlayer.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>(); // sistema de movimiento 
        controls.MovePlayer.Move.canceled += ctx => moveInput = Vector2.zero;
    }
    // se activa cuando se llama al objeto
    private void OnEnable() => controls.Enable(); // activa sistema de 

    // se desactiva cuando se deja de llamar al objeto
    private void OnDisable() => controls.Disable(); // desactiva sistema de input
   
    private void Start()
    {
        animator = GetComponent<Animator>(); // obtiene el objeto del animator
        rb = GetComponent<Rigidbody>();

        rotateSpeed = 300; // velocidad rotación (grados por segundo)
        walkSpeed = 3.5f; // velocidad al caminar
        runSpeed = 10f; // velocidad al correr 
    }

    private void Update()
    {
        isRunning = Keyboard.current.leftShiftKey.isPressed;
    }

    private void FixedUpdate()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y); // Convierte input 2D (x,y) en un vector 3D (x,0,)
        
        //bool isWalking = move.magnitude > 0.1f; // jugador moviendose (magnitud del vector)
        //animator.SetBool("isWalking", isWalking && !isRunning); // cambia parametro del animator
        //animator.SetBool("isRunning", isWalking && isRunning); // cambia parametro del animator

        float targetSpeedParam = 0f;

        if (move.magnitude > 0.1f)
            targetSpeedParam = isRunning ? 1f : 0.5f;

        float currentSpeedParam = Mathf.Lerp(animator.GetFloat("Speed"), targetSpeedParam, Time.deltaTime * 10f);
        animator.SetFloat("Speed", currentSpeedParam);

        /*if (isWalking) // si se está moviendo (input diferente a 0)
        {
            move = move.normalized;

            float currentSpeed = isRunning ? runSpeed : walkSpeed;
            Vector3 targetPos = rb.position + move * currentSpeed * Time.fixedDeltaTime; // Mueve al jugador en el mundo
            rb.MovePosition(targetPos);

            Quaternion targetRot = Quaternion.LookRotation(move);
            Quaternion smoothRot = Quaternion.RotateTowards(rb.rotation, targetRot, rotateSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(smoothRot);
        }*/

        if (move.magnitude > 0.1f)
        {
            move = move.normalized;
            float currentSpeed = isRunning ? runSpeed : walkSpeed;

            Vector3 targetPos = rb.position + move * currentSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPos);

            Quaternion targetRot = Quaternion.LookRotation(move);
            Quaternion smoothRot = Quaternion.RotateTowards(rb.rotation, targetRot, rotateSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(smoothRot);
        }
    }
}