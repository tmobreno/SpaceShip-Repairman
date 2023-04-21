using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //Management Variables
    public AudioManager manager;
    public characterController controller;
    public Animator animator;
    [SerializeField] private Rigidbody2D _rigidBody;
    public GameObject endLevelUI;
    public GameObject restartLevelUI;

    public CountdownTimer count;
    public bool CountdownActive = true;

    // Jump Variables
    public int jumpCount = 2;
    float jumpTime = 0f;
    public bool doubleJump = false;
    public bool glide = false;

    // Climb Variables
    private float vertical;
    private float speed = 4f;
    private bool isLadder;
    private bool isClimbing;

    // Movement Variables
    public float movementSpeed = 1f;
    public float jumpForce = 1f;
    bool c_FacingRight = true;
    public bool startsFlipped = false;

    // Powerup Variables
    public GameObject powerup;
    public ParticleSystem powerupEffect;


    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        endLevelUI.SetActive(false);
        restartLevelUI.SetActive(false);

        if (startsFlipped == true) {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            c_FacingRight = false;
        }
    }

    void Update()
    {
        // Moving Left and Right
        var movement = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (movement > 0)
        {
            if (!manager.IsPlaying("Walk") && animator.GetBool("isJumping") == false)
            {
                manager.Play("Walk");
            }

            animator.SetBool("isWalking", true);
            controller.move(movementSpeed);
            if (!c_FacingRight)
            {
                controller.flip();
                c_FacingRight = !c_FacingRight;
            }

        }
        if (movement < 0)
        {
            if (!manager.IsPlaying("Walk") && animator.GetBool("isJumping") == false)
            {
                manager.Play("Walk");
            }
            animator.SetBool("isWalking", true);
            controller.move(movementSpeed);
            if (c_FacingRight)
            {
                controller.flip();
                c_FacingRight = !c_FacingRight;
            }
        } else if (movement == 0 || Mathf.Abs(_rigidBody.velocity.y) > 0.001)
        {
            manager.Stop("Walk");
            animator.SetBool("isWalking", false);
        }

        // Jumping
        if (doubleJump == false && glide == false)
        {
            if (Time.time >= jumpTime)
            {
                if (Input.GetButton("Jump") && jumpCount > 1)
                {
                    manager.Play("Jump");
                    controller.jump(jumpForce);
                    animator.SetBool("isJumping", true);
                    jumpCount--;
                    jumpTime = Time.time + 0.5f;
                }
            }
        }

        // Double Jumping
        if (doubleJump == true && glide == false)
        {
            if (Time.time >= jumpTime)
            {
                if (Input.GetButton("Jump") && jumpCount > 0)
                {
                    manager.Play("Jump");
                    controller.jump(jumpForce);
                    animator.SetBool("isJumping", true);
                    jumpCount--;
                    jumpTime = Time.time + 0.5f;
                }
            }
        }

        // Gliding
        if (glide == true)
        {
            if (Input.GetButton("Jump"))
            {
                if (!manager.IsPlaying("Jump")){
                    manager.Play("Jump");
                }
                controller.glide(jumpForce);
                animator.SetBool("isJumping", true);
            }
        }

        // Climbing
        if (isLadder && Mathf.Abs(vertical) > 0f)
        {
            isClimbing = true;
        }

        if (isClimbing)
        {
            _rigidBody.gravityScale = 0f;
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, vertical * speed);
        }
        else
        {
            _rigidBody.gravityScale = 1f;
        }

        // Failure

        if (count.countdownText.text == "0" && CountdownActive == true)
        {
           Failure();
        }

    }

    //Climbing and Other Triggers
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }

        if (collision.CompareTag("Dead Zone"))
        {
            Failure();
        }

        if (collision.CompareTag("Powerup"))
        {
            powerup.SetActive(false);
            doubleJump = true;
            jumpForce = 5.5f;
            Instantiate(powerupEffect, powerup.transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
        }
    }



    public void onLanding()
    {
        animator.SetBool("isJumping", false);
        jumpCount = 2;
    }

    // Death
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Deadly")
        {
            Death();
        }
    }

    public void Death()
    {
        manager.Play("Death");
        animator.SetBool("isJumping", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isDead", true);
        this.enabled = false;
        endLevelUI.SetActive(true);
    }

    public void Failure()
    {
        manager.Play("Death");
        animator.SetBool("isJumping", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("hasFailed", true);
        this.enabled = false;
        restartLevelUI.SetActive(true);
    }
}
