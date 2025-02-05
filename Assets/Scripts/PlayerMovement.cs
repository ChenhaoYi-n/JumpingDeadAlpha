
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerMovement : MonoBehaviour
{   
    bool isGrounded = false;
    [SerializeField] GameObject reticle;
    [SerializeField] GameObject reticlecenter;
    // [SerializeField] TextMeshProUGUI stepsText;
    //bool spacepressed = false;

    Rigidbody2D rb;
    bool facingRight = true;

    bool wallSliding;
    [SerializeField] float wallSlidingSpeed = 2f;
    [SerializeField] float minRetAngle;
    [SerializeField] float maxRetAngle;

    bool wallJumping;
    float wallJumpDirection;
    [SerializeField] float wallJumpTime = 0.2f;
    [SerializeField] float wallJumpCounter;
    [SerializeField] float wallJumpDuration = 0.4f;
    [SerializeField] Vector2 wallJumpPower = new Vector2(8f, 16f);

    Vector2 playerpos;
    Vector2 reticlepos;
    Vector2 direction;
    [SerializeField] float speed;
    [SerializeField] float regGrav = 1.0f;
    [SerializeField] float wallGrav = 0.0f;
    [SerializeField] int numJumps = 5;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform wallCheck;
    [SerializeField] LayerMask wallLayer;

    [SerializeField] float minReticleDistance;  
    [SerializeField] float maxReticleDistance; 
    [SerializeField] float reticleSpeed;
    private bool increasingDistance = true;          
    private float currentReticleDistance; 

    bool isTouchingLeftWall = false; 
    bool isTouchingRightWall = false;
    int currentJumps;
    

    bool onWall = false;
   
    //[SerializeField] float acceleration = 1f;
    //[SerializeField] float maxspeed = 300f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentJumps = numJumps;
        currentReticleDistance = minReticleDistance;
    }

    // Update is called once per frame
    void Update()
    {   minReticleDistance = 5.0f;  
        maxReticleDistance = 10.0f; 
        reticleSpeed = 3.0f;

        if (currentJumps <= 0)
        {
            RestartGame();
        }

        playerpos = (Vector2)transform.position;
        reticlepos = (Vector2)reticle.transform.position;
        direction = reticlepos - playerpos;
        
        if (Input.GetKey("space") && isGrounded)
        {
            if (increasingDistance)
            {   
                currentReticleDistance += reticleSpeed * Time.deltaTime;
                if (currentReticleDistance >= maxReticleDistance)
                {   
                    increasingDistance = false;
                }
            }
            // else
            // {
            //     currentReticleDistance -= reticleSpeed * Time.deltaTime;
            //     if (currentReticleDistance <= minReticleDistance)
            //     {
            //         increasingDistance = true;
            //     }
            // }
            reticle.transform.position = (Vector2)transform.position + direction.normalized * currentReticleDistance / 5.0f;
        }

        if (Input.GetKeyUp("space") && isGrounded)
        {
            Vector2 jumpDirection = (Vector2)(reticle.transform.position - transform.position).normalized;

            float jumpForce = currentReticleDistance * speed;

            rb.AddForce(jumpDirection * jumpForce / 200.0f, ForceMode2D.Impulse);

            currentReticleDistance = minReticleDistance;
            // Vector2 initialDirection = (Vector2)(reticlecenter.transform.position - transform.position).normalized;
            reticle.transform.position = (Vector2)transform.position + direction.normalized * currentReticleDistance / 5.0f;
            increasingDistance = true;
        }

        // if (Input.GetKeyDown("space") && (IsGrounded()))
        // {
        //     playerpos = (Vector2)transform.position;
        //     reticlepos = (Vector2)reticle.transform.position;
        //     direction = reticlepos - playerpos;
        //     rb.AddForce(direction.normalized * speed);
           
        // }
        if(Input.GetKeyUp("space") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        /*
        if (Input.GetKeyUp("space") && currentJumps > 0 && onWall) 
        {
            playerpos = (Vector2)transform.position;
            reticlepos = (Vector2)reticle.transform.position;
            direction = reticlepos - playerpos;
            if (currentJumps < numJumps)
            {
                rb.velocity = Vector3.zero;
            }
            
            rb.AddForce(direction.normalized * speed);
            
        }
        */
        if (Input.GetKeyUp("r"))
        {
            string currentscene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentscene);
        }
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        WallSlide();
        WallJump();
        Flip();
    }

    void Flip()
    {
        if ((facingRight && reticle.transform.position.x-transform.position.x < 0) || (!facingRight && reticle.transform.position.x - transform.position.x >0))
        {
            facingRight = !facingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
            rb.gravityScale = regGrav;
            isTouchingLeftWall = false;
            isTouchingRightWall = false;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            isGrounded = true;
            rb.gravityScale = 0.1f;
            reticleSpeed = 100.0f;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Wall"))
        {
            isGrounded = false;
            rb.gravityScale = regGrav;
            reticleSpeed = 6.0f;
        }
    }

    bool IsGrounded()
    {
        return true;
    }

    bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    void WallSlide()
    {
        if(IsWalled() && !IsGrounded())
        {
            wallSliding = true;
            //rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            if (facingRight)
            {
                minRetAngle = 10f;
                maxRetAngle = 170f;
            }
            else
            {
                minRetAngle = -170f;
                maxRetAngle = -10f;
            }
            //reticlecenter.transform.rotation = new Quaternion(0, 0, Mathf.Clamp(reticlecenter.transform.rotation.z, minRetAngle, maxRetAngle), 1);
            var targetRotation = Quaternion.Euler(Vector3.forward * Mathf.Clamp(reticlecenter.transform.rotation.z, minRetAngle, maxRetAngle));

            reticlecenter.transform.rotation = Quaternion.Lerp(reticlecenter.transform.rotation, targetRotation, 30f*Time.deltaTime);
        }
        else
        {
            wallSliding = false;
            //rb.constraints = RigidbodyConstraints2D.None;
            //rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
    
    void WallJump()
    {
        if(wallSliding && Input.GetKeyDown("space"))
        {
            rb.velocity = Vector2.zero;
            playerpos = (Vector2)transform.position;
            reticlepos = (Vector2)reticle.transform.position;
            direction = reticlepos - playerpos;
            rb.AddForce(direction.normalized * speed);
            
            
        }
        /*
        if (wallSliding)
        {
            wallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpCounter = wallJumpTime;
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpCounter -= Time.deltaTime;
        }
       
        if (Input.GetKeyDown("space") && wallJumpCounter > 0f)
        {
            wallJumping = true;
            rb.velocity = Vector2.zero;
            playerpos = (Vector2)transform.position;
            reticlepos = (Vector2)reticle.transform.position;
            direction = reticlepos - playerpos;
            rb.AddForce(direction.normalized * speed);
            wallJumpCounter = 0f;

            Invoke(nameof(StopWallJumping), wallJumpDuration);
        }
        */
    }

    void StopWallJumping()
    {
        wallJumping = false;
    }
    
    /*
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            reticlecenter.transform.rotation = new Quaternion(0, 0, reticlecenter.transform.rotation.z * -1, 1);
            
            currentJumps -= 1;
            onWall = true;
            if (rb.velocity.y < 0)
            {
                rb.gravityScale = wallGrav;
            }
            
        }
    }
   
    
    private void OnCollisionStay2D(Collision2D col)
    {
        
        if (col.gameObject.CompareTag("Floor"))
        {
            onWall = true;
            currentJumps = numJumps;
        }
        
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            rb.gravityScale = regGrav;
            onWall = true;
        }
        if (col.gameObject.CompareTag("Floor"))
        {
            currentJumps = numJumps;
            onWall = true;
        }
        onWall = false;
    }
    */

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
