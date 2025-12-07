using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private float speed = 8f;
    [SerializeField] private bool isFalling;
    [SerializeField] private bool isGrounded;
    private float horizontalInput;
    private CapsuleCollider2D capsuleCollider;    
    private float wallDetectionRange = 0.477f;
    private float groundDetectionRange = 0.1f;
    private float wallSlidingSlowedSpeed = 0.8f;       //Normal value 0.8f
    private bool isWallSliding;
    
    //Jump
    [SerializeField] private bool isJumping;
    [SerializeField] private float jumpTimer;
    [SerializeField] private float jumpCooldown = 0.25f;
    [SerializeField] private Vector2 wallJumpForce = new Vector2(7f,5f);
    private bool isWallJumping;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float wallJumpBlockTime=0.4f;

    private bool isAttacking;
    public bool isHurt;
    private bool isDead;


//ANIMATION
    private Animator animator;
    private string currentAnimation ="";



//CLIMB
       private bool greenBox, redBox;
/*       public float redXOffset;
       public float redYOffset;
       public float redXSize; 
       public float redYSize; 
       public float greenXOffset; 
       public float greenYOffset; 
       public float greenXSize;
       public float greenYSize;
*/
       private bool isGrabbing;



    void Awake()  
    {
        //Grab references for rigid body and animator from object
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>(); 
        
    }

    void Start()
    {
        UpdateAnimation("Idle");
    }

    //Get input
    void Update()  
    {

        if (!isDead && !isHurt && !isWallJumping)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            
            if (!isWallJumping && !isGrabbing && !isAttacking) Flip();
            //Climb();
            IsGrounded();
            
            Idle();
            Jump();
            WallSlide();
            Falling();
            Running();
            Attack();
        }
    }

    //Apply inputs to our character
    void FixedUpdate()
    {
    if (!isWallJumping && !isAttacking && !isHurt)
        {
            body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);
        }  
            //Cooldown only decreases if isGrounded
            if(IsGrounded() || isWallSliding)
            {
                jumpTimer -= Time.fixedDeltaTime;
            }
            else
            {
                jumpTimer = jumpCooldown;
            }
  
    }
   

    private void Flip()
    {
         if (horizontalInput > 0 && transform.localScale.x > 0 ||
            horizontalInput < 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }

    private void Idle()
    {
        if (Math.Abs(horizontalInput) <= 0.01f && IsGrounded() && !isAttacking)
        {
            UpdateAnimation("Idle");
            //isJumping = false;               
        }    
    }
    private void Running()
    {
        if (Math.Abs(horizontalInput) > 0.01f && IsGrounded() && !isAttacking){
            UpdateAnimation("Running");
            isJumping = false; 
        }
    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && jumpTimer <= 0.1f && !isGrabbing && !isAttacking)
        {

            
            
            UpdateAnimation("RunningJump");
            isJumping = true;
            if (IsGrounded())
                {
                    body.linearVelocity = new Vector2(body.linearVelocityX, jumpForce);
                    jumpTimer = jumpCooldown;
                    UpdateAnimation("RunningJump");
                                    
                }
                /*else*/ if(isWallSliding)
                {
                    int direction = GetWallJumpDirection();
                    WallJump(direction);
                }
        }
    }

    public void Attack()
    {
        if (IsGrounded() && Input.GetKey(KeyCode.E) && !isAttacking)
        {   
            StartCoroutine(PerformAttack());

        }
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true; // Empieza el ataque

        body.linearVelocity = Vector2.zero;
        UpdateAnimation("Attack"); 
        yield return new WaitForSeconds(0.5f);
        //yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
        isAttacking = false; 
    
    }

    IEnumerator DoClimb()
    {     
        UpdateAnimation("Climb");
        yield return new WaitForSeconds(1.1f);
        //Change position
        transform.position = new Vector2(transform.position.x + (-Mathf.Sign(transform.localScale.x) * 1f), transform.position.y + 1f);
        body.gravityScale = 1f;
        isGrabbing = false;
        //Invoke(nameof(DoClimb), 1.1f);
    }

    

    public void WallJump(int direction)
    {
        
         if (isWallSliding)
        {
            isWallJumping=false;
            CancelInvoke(nameof(StopWallJumping));
        }
            
        isWallJumping=true;
        body.linearVelocity = new Vector2(direction * wallJumpForce.x, wallJumpForce.y);
        jumpTimer = jumpCooldown;
        

        //Flip
        if (transform.localScale.x != direction)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        //Jump duration
        Invoke(nameof(StopWallJumping), wallJumpBlockTime);
        
        UpdateAnimation("RunningJump");
    }
      private void StopWallJumping()
    {
        isWallJumping = false;
        isJumping = false;
    }

    private void Falling()
    {
        if (body.linearVelocityY < 0 && !IsGrounded() && !isWallSliding /*&& !isWallJumping*/)
        {
            isFalling=true;                     
            UpdateAnimation("Falling");
        }
        else
        {
           isFalling=false;
        
        }
    }

    private void WallSlide()
    {
        if((RightWallDetected() || LeftWallDetected()) && !IsGrounded() && !isWallJumping)
        {
            isWallSliding=true;
            body.linearVelocityY *= wallSlidingSlowedSpeed;
            UpdateAnimation("WallSlide"); 
        }
        else
        {
            isWallSliding=false;
        }               
    }
    private bool RightWallDetected()
    {
        //Check is player is aiming right
             if( Math.Sign(transform.localScale.x) != 1 ){
            RaycastHit2D rayscastHit2D = Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.right, wallDetectionRange, LayerMask.GetMask("Wall"));
            return rayscastHit2D.collider !=null;
        }
        return false;
    }

    private bool LeftWallDetected()
    {
        //Check is player is aiming left           
        if( Math.Sign(transform.localScale.x) != -1 ){
        RaycastHit2D rayscastHit2D = Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.left, wallDetectionRange, LayerMask.GetMask("Wall"));
        return rayscastHit2D.collider !=null;
        }
        return false;
    }
    private int GetWallJumpDirection()
    {
        if (RightWallDetected()) return -1;
        if (LeftWallDetected()) return 1;
        return 0;
    }

    private void UpdateAnimation(string animation)
    {
        if (currentAnimation != animation)
        {
            currentAnimation = animation;
            animator.Play(animation);   //No tiene transicion  
        }
    }


    private bool IsGrounded()
    {
        //No puedo usar raycast porque en el filo no detecta ground y hace Falling continuo
        Vector3 size = capsuleCollider.bounds.size;
        size.x*=0.6f;
        return Physics2D.BoxCast(capsuleCollider.bounds.center, size, 0f, Vector2.down, groundDetectionRange, LayerMask.GetMask("Ground"));
    }

   


private void OnDrawGizmos()
{
    //capsuleCollider.bounds.center, capsuleCollider.bounds.size, 0f, Vector2.down, groundDetectionRange, LayerMask.GetMask("Ground")
    Gizmos.color = Color.red;
   
    //Gizmos.DrawWireCube(capsuleCollider.bounds.center, capsuleCollider.bounds.size);
    Gizmos.color = Color.purple;
    
    Vector3 endPoint = capsuleCollider.bounds.center + (Vector3.down * groundDetectionRange);
    
    Vector3 size = capsuleCollider.bounds.size;
    size.x*=0.3f;
    Gizmos.DrawWireCube(endPoint,size);
   
   /* 
    
    // Punto final: origen + dirección * distancia
    Vector3 leftEndPoint = capsuleCollider.bounds.center + Vector3.left * wallDetectionRange;

    // Línea del raycast
    Gizmos.DrawLine(capsuleCollider.bounds.center, leftEndPoint);


    Gizmos.color = Color.navyBlue;

    // Punto final: origen + dirección * distancia
    Vector3 rightEndPoint = capsuleCollider.bounds.center + Vector3.right * wallDetectionRange;

    // Línea del raycast
    Gizmos.DrawLine(capsuleCollider.bounds.center, rightEndPoint);



    //Climb detector
    Gizmos.color = Color.red;
    Gizmos.DrawCube(new Vector2(transform.position.x + (-0.47f * transform.localScale.x), transform.position.y + -0.14f), new Vector2(0.43f, 0.05f));

    Gizmos.color = Color.green;
    Gizmos.DrawCube(new Vector2(transform.position.x + (-0.47f * transform.localScale.x), transform.position.y + 0.43f), new Vector2(0.43f, 0.05f));
*/
        

    /*Ground Raycast
    Gizmos.color = Color.orange;
    // Punto final: origen + dirección * distancia
    Vector3 groundPoint = capsuleCollider.bounds.center + Vector3.down * groundDetectionRange;

    // Línea del raycast
    Gizmos.DrawLine(capsuleCollider.bounds.center, groundPoint);*/

/*
    Gizmos.color = Color.orange;
    
    Gizmos.DrawWireCube(capsuleCollider.bounds.center, capsuleCollider.bounds.size);

    Vector3 end = capsuleCollider.bounds.center + Vector3.down * groundDetectionRange;
    Gizmos.DrawWireCube(end, capsuleCollider.bounds.size);
*/
}

 
}
