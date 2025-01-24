using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class char_movement : MonoBehaviour
{

    [SerializeField] private Camera cam;
    private CharacterController controller;

    [SerializeField] private float moveSpeed;
    
    private float gravity = 9.8f;

    private Vector2 moveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 moveVelocity = (cam.transform.right * moveInput.x + cam.transform.forward * moveInput.y + Vector3.down*gravity) * Time.deltaTime * moveSpeed;
        controller.Move(moveVelocity);
        
    }

    public void GetMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

    }
}
