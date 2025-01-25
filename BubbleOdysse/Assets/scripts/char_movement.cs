using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class char_movement : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Camera cam;
    private CharacterController controller;
    [SerializeField] private GameObject bubble;


    [Header("Movement Variables")]
    private Vector2 moveInput;
    [SerializeField] private float dashForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    private float gravity = 9.8f;
    private bool isGrounded;

    float velocity;


    private int mode = 0;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        GroundCheck();
    }

    private void Move()
    {
        Vector3 moveVelocity = (cam.transform.right * moveInput.x + cam.transform.forward * moveInput.y + Vector3.down) * Time.deltaTime * moveSpeed;
        controller.Move(moveVelocity);
        moveVelocity.y = 0;
        Rotate(moveVelocity);
        
    }

    private void Rotate(Vector3 target)
    {
        transform.LookAt(transform.position + target);
    }

    public void GetMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

    }

    public void SwitchMode(InputAction.CallbackContext context)
    {
        //switch between modes and change the mode variable and move speed

        //Modes: 0 - Dash, 1 - Floating, 2 - SummonBubble
        if(context.performed){
            mode = (mode + 1) % 3;
            if(mode == 0){
            moveSpeed = 10;
            }
            else if(mode == 1){
            moveSpeed = 3;
            }
            else if(mode == 2){
            moveSpeed = 5;
            }    
        } 
          

    }
    
    public void ActionInput(InputAction.CallbackContext context)
    {

        if(context.performed){
            //Check which mode is active and call the corresponding function
            if(mode == 0){
                Dash();
            }
            else if(mode == 1){
                Floating();
            }
            else if(mode == 2){
                SummonBubble();
            }
        }
    }
    
    public void Jump(InputAction.CallbackContext context){
        //Lets the player jump normally
        if(isGrounded){
            controller.Move(Vector3.up * jumpForce * Time.deltaTime);
        }

        

    }

    private void Floating(){
        // Lets the player float in the air while holding spacbar and move slowly in the air
        print("Floating");
    }

    private void Dash(){
        
        //Lets the player dash in the direction he is facing
        controller.Move(transform.forward * dashForce * Time.deltaTime);

        print("Dashing");


    }

    private void SummonBubble(){

        print("Summoning Bubble");
        
        //Check if the bubble is already summoned
             

        //Summon the bubble in front of the player

        //bubble functions as a platform that the player can jump on
        

    }

    private void GroundCheck(){
        // Check if the player is standing on the ground iwth a raycast
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 1.3f)){
            isGrounded = true;
        }
        else{
            isGrounded = false;
        }
        
        
    }

}
