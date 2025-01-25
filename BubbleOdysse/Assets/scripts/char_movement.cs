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
    [SerializeField] private float initialJumpForce;
    private float jumpForce;

    private int countScore = 0;



    //SOUND 
    [SerializeField] private AudioClip dashSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip plopSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip floatSound;
    [SerializeField] private AudioClip[] runSound;

    //private float gravity = 9.8f;
    private bool isGrounded;

    float velocity;

    bool isFloating = false;


    private int mode = 0;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        jumpForce = initialJumpForce;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        SoundFXManager.instance.PlayRandomSoundFXClip(runSound, transform, 1f);
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

            //Sound
            SoundFXManager.instance.PlaySoundFXClip(jumpSound, transform, 1f);
        }

        

    }

    private void Floating(){
        // Lets the player toggle floating mode wich gives him the current height +2 and stay there until he toggles it off

       if(!isFloating){
         if(!isGrounded){
            float currentHeight = transform.position.y;
            transform.position = new Vector3(transform.position.x, currentHeight + 2, transform.position.z);
            isFloating = true;
            print("FloatingOn");

                //Sound
                SoundFXManager.instance.PlaySoundFXClip(floatSound, transform, 1f);
            } 
       }
       else isFloating = false;
       print("FloatingOff");
        

        
    }

    private void Dash(){
        
        //Lets the player dash in the direction he is facing
        controller.Move(transform.forward * dashForce * Time.deltaTime);
        SoundFXManager.instance.PlaySoundFXClip(dashSound, transform, 1f);

    }

    private void SummonBubble(){

        print("Summoning Bubble");

        //Check if the bubble is already summoned


        //Summon the bubble in front of the player

        //bubble functions as a platform that the player can jump on

        //Sound
        SoundFXManager.instance.PlaySoundFXClip(hitSound, transform, 1f);


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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Check if the player is colliding with the bubble
        if(hit.gameObject.tag == "BubbleBounce"){
            //If the player is colliding with the bubble, play the plop sound
            SoundFXManager.instance.PlaySoundFXClip(plopSound, transform, 1f);
            jumpForce = jumpForce * 5;
            print(jumpForce);
            controller.Move(Vector3.up * jumpForce * Time.deltaTime);
            jumpForce = initialJumpForce;
            

        }
        else if(hit.gameObject.tag == "BubbleCollect"){
            //If the player is colliding with the bubble, play the plop sound
            SoundFXManager.instance.PlaySoundFXClip(plopSound, transform, 1f);
            countScore++;
            print(countScore);
        }
        else if(hit.gameObject.tag == "DEATH"){
            GameOver();
        }

    }

    public void GameOver(){
            //End the game and show the score
            SoundFXManager.instance.PlaySoundFXClip(hitSound, transform, 1f);
            print("Game Over");
    }
}