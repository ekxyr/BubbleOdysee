using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine.InputSystem;
using UnityEngine.Animations;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using System;

public class char_movement : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Camera cam;
    private CharacterController controller;
    [SerializeField] private GameObject bubble;
    [SerializeField] GameObject _gameOver;
    [SerializeField] GameObject _youWon;
    [SerializeField] GameObject _HideMouse;
    [SerializeField] GameObject _ShowMouse;
    Boolean over = false;
    Boolean win = false;


    [Header("Movement Variables")]
    private Vector2 moveInput;
    private Vector3 moveDirection;
    [SerializeField] private float dashForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float initialJumpForce;
    private float jumpForce;



    //ANIMATION
    [SerializeField] private Animator animator;
    
    
    //SOUND 
    [SerializeField] private AudioClip dashSound;       //DONE
    [SerializeField] private AudioClip jumpSound;       //DONE
    [SerializeField] private AudioClip plopSound;       //DONE
    [SerializeField] private AudioClip swingSound;      //DONE
    [SerializeField] private AudioClip floatSound;      //DONE
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip winSound;        //DONE
    [SerializeField] private AudioClip coinSound;       //DONE
    [SerializeField] private AudioClip[] deathSound;    //DONE

    private bool isRepeating = false;

    private float gravity = -9.81f;
    [SerializeField] private float initialGravityMultiplier = 1f;
    private float gravityMultiplier;
    private float velocity;

    bool isFloating = false;
    private int mode = 0;
    private bool isColliding;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        controller = GetComponent<CharacterController>();
        jumpForce = initialJumpForce;
        gravityMultiplier = initialGravityMultiplier;
        InvokeRepeating("RepeatFunction", 0f, 0.5f);
        
    }

    // Update is called once per frame
    void Update()
    {
        isColliding = false; //check if colliding
        ApplyRotate();
        ApplyGravity();
        
        ApplyMove();

        if(IsGrounded() && isFloating) Floating(); 
        

        
        //IsGrounded();
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        moveDirection  = new Vector3(moveInput.x, 0, moveInput.y);
        animator.SetBool("IsRunning", true);
        SoundFXManager.instance.PlaySoundFXClip(walkSound, transform, 1f);
        
        if (moveInput.x == 0 && moveInput.y == 0)
        {
            animator.SetBool("IsRunning", false);
            isRepeating = false;
        }
    }

     void RepeatFunction()
    {
        if(animator.GetBool("IsRunning") == false)
        {
            return;
        }
   
         SoundFXManager.instance.PlaySoundFXClip(walkSound, transform, 1f);
         
    }


    private void ApplyRotate()
    {
        if(moveInput.sqrMagnitude == 0) return;

       
        moveDirection = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0) * new Vector3(moveInput.x, 0, moveInput.y);
        var targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 720 * Time.deltaTime);

    }
    
    private void ApplyMove()
    {
        controller.Move(moveDirection * Time.deltaTime * moveSpeed);
    }

    private void ApplyGravity()
    {
        
        if(IsGrounded() && velocity < 0)
        {
           
            velocity = -1.0f;
        }
        else
        {
            velocity += gravity * gravityMultiplier * Time.deltaTime;
        }

        moveDirection.y = velocity;
        //controller.Move(Vector3.up * velocity * Time.deltaTime);
    }

    
    public void SwitchMode(InputAction.CallbackContext context)
    {
        //switch between modes and change the mode variable and move speed

        //Modes: 0 - Dash, 1 - Floating, 2 - SummonBubble
        if(context.performed){
            mode = (mode + 1) % 3;
            if(mode == 0){
            moveSpeed = 7;
            print("Mode: Dash");
            }
            else if(mode == 1){
            moveSpeed = 6;
            print("Mode: Floating");
            }
            else if(mode == 2){
            moveSpeed = 6;
            print("Mode: Summon Bounce Bubble");
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
                animator.SetBool("IsSwinging", false);
            }
        }
    }
    
    public void Jump(InputAction.CallbackContext context){
        //Lets the player jump normally
        
        if(!context.performed) return;
        
        if(!IsGrounded()) return;   

        velocity += jumpForce;
        animator.SetBool("IsJumping",true);
        SoundFXManager.instance.PlaySoundFXClip(jumpSound, transform, 1f);

    }

    private void Floating(){
        // Lets the player toggle floating mode wich gives him the current height +2 and stay there until he toggles it off

       if(!isFloating){
        if(!IsGrounded()){
                gravityMultiplier = 0.03f;
                isFloating = true;
                animator.SetBool("IsFloating", true);
                velocity = 0;
                //Sound
                SoundFXManager.instance.PlaySoundFXClip(floatSound, transform, 1f);
            } 
       }
        else {
            isFloating = false;
            animator.SetBool("IsFloating", false);
            gravityMultiplier = initialGravityMultiplier;
            

       }        
    }

    private void Dash(){
        
        //Lets the player dash in the direction he is facing
        if(IsGrounded()){
            controller.Move(transform.forward * dashForce * Time.deltaTime);
            SoundFXManager.instance.PlaySoundFXClip(dashSound, transform, 1f);
        }
        

    }

    private void SummonBubble(){
<<<<<<< Updated upstream
      
=======

        print("Summoning Bubble");

        animator.SetBool("IsSwinging", true);
>>>>>>> Stashed changes
        //Summon the bubble in front of the player
        bubble.transform.position = transform.position + transform.forward * 2;
        bubble.SetActive(true);

        //bubble functions as a platform that the player can jump on

        //Sound
        SoundFXManager.instance.PlaySoundFXClip(swingSound, transform, 1f);

        animator.SetBool("IsSwinging", false);
    }

    private bool IsGrounded(){
        animator.SetBool("IsJumping", false);
        return controller.isGrounded;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Check if the player is colliding with the bubble
        if (hit.gameObject.tag == "BubbleBounce")
        {
            //If the player is colliding with the bubble, play the plop sound
            SoundFXManager.instance.PlaySoundFXClip(plopSound, transform, 1f);
            
            bubble.SetActive(false);
            velocity = jumpForce * 1.7f;
        }
        
        
        
        else if (hit.gameObject.CompareTag("Coin"))
        {
            //Collision detection with Coins + prevent from trigger multiple times when collecting
            if(isColliding) return;
            isColliding = true;
            Destroy(hit.gameObject);
            WorldScript.collectCoin();
            SoundFXManager.instance.PlaySoundFXClip(coinSound, transform, 1f);
        }
        
        else if(hit.gameObject.tag == "DeathPlane"){
            GameOver();
        }
        
        else if (hit.gameObject.CompareTag("EndBox") && WorldScript.collectedCoins >= 4)
        {
            print("YOU HAVE WON!!!!");
            SoundFXManager.instance.PlaySoundFXClip(winSound, transform, 1f);
            win = true;
            if (win == true)
            {
                _HideMouse.SetActive(false);
                _ShowMouse.SetActive(true);
                _youWon.SetActive(true);
            }
        }
        
    }

    public void GameOver(){
            //End the game and show the score
            SoundFXManager.instance.PlayRandomSoundFXClip(deathSound, transform, 1f);
        over = true;
        if (over == true)
        {
            _HideMouse.SetActive(false);
            _ShowMouse.SetActive(true);
            _gameOver.SetActive(true);
        }
        print("Game Over");
    }
}