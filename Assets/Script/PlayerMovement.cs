using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControl controls; // llamada archivo de InputSystem generado por PlayerControl.inputactions
    private Vector2 moveInput; // llamada valor del movimiento leído del InputSystem (WASD / Joystick)

    private Animator animator; // llamada valor de rotacion leído del InputSystem (AD / Joystick)
    private Rigidbody rb;

    public float rotateSpeed = 120f; // velocidad rotación (grados por segundo)
    

    private void Awake()
    {   
        controls = new PlayerControl(); // inicia el sistema de controles
        
        controls.MovePlayer.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>(); // sistema de movimiento 
        controls.MovePlayer.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnEnable() => controls.Enable(); // activa sistema de input
    // se activa cuando se llama al objeto
    private void OnDisable() => controls.Disable(); // desactiva sistema de input
    // se desactiva cuando se deja de llamar al objeto
    private void Start()
    {
        animator = GetComponent<Animator>(); // obtiene el objeto del animator
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y); // Convierte input 2D (x,y) en un vector 3D (x,0,z)

        bool isWalking = move.magnitude > 0.1f; // jugador moviendose (magnitud del vector)
        animator.SetBool("isWalking", isWalking); // cambia parametro del animator
        
        if (isWalking) // si se está moviendo (input diferente a 0)
        {
            move = move.normalized;

            Vector3 targetPos = rb.position + move * 3f * Time.fixedDeltaTime; // Mueve al jugador en el mundo
            rb.MovePosition(targetPos);

            Quaternion targetRot = Quaternion.LookRotation(move);
            Quaternion smoothRot = Quaternion.RotateTowards(rb.rotation, targetRot, rotateSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(smoothRot);
        }
    }
}
