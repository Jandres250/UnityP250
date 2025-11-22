using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // llamada archivo de InputSystem generado por PlayerControl.inputactions
    private PlayerControl controls;
    // llamada valor del movimiento leído del InputSystem (WASD / Joystick)
    private Vector2 moveInput;
    // llamada valor de rotacion leído del InputSystem (AD / Joystick)
    private Animator animator;
    public float rotateSpeed = 0.1f; // velocidad rotación (grados por segundo)

    private void Awake()
    {   
        // inicia el sistema de controles
        controls = new PlayerControl();
        
        // sistema de movimiento 
        controls.MovePlayer.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.MovePlayer.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    // se activa cuando se llama al objeto
    private void OnEnable()
    {
        controls.Enable(); // activa sistema de input
    }

    // se desactiva cuando se deja de llamar al objeto
    private void OnDisable()
    {
        controls.Disable(); // desactiva sistema de input
    }

    private void Start()
    {
        // obtiene el objeto del animator
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // jugador moviendose (magnitud del vector)
        bool isWalking = moveInput.magnitude > 0.1f;
        // cambia parametro del animator
        animator.SetBool("isWalking", isWalking);

        // Convierte input 2D (x,y) en un vector 3D (x,0,z)
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        // si se está moviendo (input diferente a 0)
        if (move.sqrMagnitude > 0.01f) 
        {
            // Rotamos hacia esa dirección
            Quaternion targetRot = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
            
            // Mueve al jugador en el mundo
            transform.Translate(move * 3f * Time.deltaTime, Space.World);
        }
    }
}
