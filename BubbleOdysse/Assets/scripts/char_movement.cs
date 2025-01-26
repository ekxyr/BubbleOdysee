using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine.InputSystem;
using UnityEngine.Animations;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public class char_movement : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Camera cam;
    private CharacterController controller;
    [SerializeField] private GameObject bubble;

    //SoundFX
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip dashSound;
    [SerializeField] private AudioClip floatSound;
    [SerializeField] private AudioClip plopSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip[] runSounds;


    [Header("Movement Variables")]
    private Vector2 moveInput;
    private Vector3 moveDirection;
    [SerializeField] private float dashForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float initialJumpForce;
    private float jumpForce;

    private int countScore = 0;


    //ANIMATION
    [SerializeField] private Animator animator;
    
    
    //SOUND 
    [SerializeField] private AudioClip dashSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip plopSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip floatSound;
    [SerializeField] private AudioClip[] runSound;

    private float gravity = -9.81f;
    [SerializeField] private float baseGravityMultiplier = 1f;
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
        gravityMultiplier = baseGravityMultiplier;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream
        isColliding = false; //check if colliding
        ApplyRotate();
        ApplyGravity();
        
        ApplyMove();

        if(IsGrounded() && isFloating) Floating();
        

        
        //IsGrounded();
=======
        Move();
        GroundCheck();
    }

    private void Move()
    {
 
        Vector3 moveVelocity = (cam.transform.right * moveInput.x + cam.transform.forward * moveInput.y + Vector3.down) * Time.deltaTime * moveSpeed;
        controller.Move(moveVelocity);
        moveVelocity.y = 0;
        Rotate(moveVelocity);

>>>>>>> Stashed changes
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
<<<<<<< Updated upstream
        moveDirection  = new Vector3(moveInput.x, 0, moveInput.y);
        animator.SetBool("IsRunning", true);
        SoundFXManager.instance.PlayRandomSoundFXClip(runSound, transform, 1f);
        if (moveInput.x == 0 && moveInput.y == 0)
        {
            animator.SetBool("IsRunning", false);
        }
=======
        SoundFXManager.instance.PlayRandomSoundFXClip(runSounds, transform, 1f);

>>>>>>> Stashed changes
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
            moveSpeed = 10;
            }
            else if(mode == 1){
            moveSpeed = 10;
            }
            else if(mode == 2){
            moveSpeed = 10;
            }    
        } 
        gravityMultiplier = baseGravityMultiplier;
          

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
<<<<<<< Updated upstream
        
        if(!context.performed) return;
        
        if(!IsGrounded()) return;   
=======
        if(isGrounded){
            controller.Move(Vector3.up * jumpForce * Time.deltaTime);
            //Sound
            SoundFXManager.instance.PlaySoundFXClip(jumpSound, transform, 1f );
        }
>>>>>>> Stashed changes

        velocity += jumpForce;
        

    }

    private void Floating(){
<<<<<<< Updated upstream
        // Lets the player toggle floating mode wich gives him the current height +2 and stay there until he toggles it off

       if(!isFloating){
        
            gravityMultiplier = 0.01f;
            jumpForce = 0f;
            isFloating = true;
            controller.Move(Vector3.up * jumpForce * Time.deltaTime);

                //Sound
                SoundFXManager.instance.PlaySoundFXClip(floatSound, transform, 1f);
            
       }
        else {
            isFloating = false;
            gravityMultiplier = baseGravityMultiplier;
            jumpForce = initialJumpForce;
            

       }
        

        
=======
        // Lets the player float in the air while holding spacbar and move slowly in the air
        print("Floating");
        //Sound
        SoundFXManager.instance.PlaySoundFXClip(floatSound, transform, 1f);
>>>>>>> Stashed changes
    }

    private void Dash(){
        
        //Lets the player dash in the direction he is facing
<<<<<<< Updated upstream
        if(IsGrounded()){
            controller.Move(transform.forward * dashForce * Time.deltaTime);
            SoundFXManager.instance.PlaySoundFXClip(dashSound, transform, 1f);
        }
        
=======
        controller.Move(transform.forward * dashForce * Time.deltaTime);

        print("Dashing");

        //Sound
        SoundFXManager.instance.PlaySoundFXClip(dashSound, transform, 1f);

>>>>>>> Stashed changes

    }

    private void SummonBubble(){

        print("Summoning Bubble");

        
        //Summon the bubble in front of the player
        bubble.transform.position = transform.position + transform.forward * 2;
        bubble.SetActive(true);

        //bubble functions as a platform that the player can jump on

        //Sound
        SoundFXManager.instance.PlaySoundFXClip(hitSound, transform, 1f);


    }

    private bool IsGrounded(){
        return controller.isGrounded;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Check if the player is colliding with the bubble
        if (hit.gameObject.tag == "BubbleBounce")
        {
            //If the player is colliding with the bubble, play the plop sound
            SoundFXManager.instance.PlaySoundFXClip(plopSound, transform, 1f);
            jumpForce = jumpForce * 5;
            print(jumpForce);
            controller.Move(Vector3.up * jumpForce * Time.deltaTime);
            jumpForce = initialJumpForce;
            bubble.SetActive(false);
        }
        
        
        else if(hit.gameObject.tag == "BubbleCollect"){
            //If the player is colliding with the bubble, play the plop sound
            SoundFXManager.instance.PlaySoundFXClip(plopSound, transform, 1f);
            countScore++;
            print(countScore);
        }
        
        else if (hit.gameObject.CompareTag("Coin"))
        {
            //Collision detection with Coins + prevent from trigger multiple times when collecting
            if(isColliding) return;
            isColliding = true;
            Destroy(hit.gameObject);
            WorldScript.collectCoin();
            
        }
        
        else if(hit.gameObject.tag == "DeathPlane"){
            GameOver();
        }
        
        else if (hit.gameObject.CompareTag("EndBox") && WorldScript.collectedCoins >= 4)
        {
            print("YOU HAVE WON!!!!");
        }
        
    }

    public void GameOver(){
            //End the game and show the score
            SoundFXManager.instance.PlaySoundFXClip(hitSound, transform, 1f);
            print("Game Over");
    }
}