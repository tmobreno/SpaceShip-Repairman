using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class characterController : MonoBehaviour
{

    private Rigidbody2D _rigidBody;
    public UnityEvent OnLandEvent;

    private bool m_Grounded;
    [SerializeField] private Transform m_GroundCheck;
    const float k_GroundedRadius = .1f;
    [SerializeField] private LayerMask m_WhatIsGround;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    public void move(float movementSpeed)
    {
        var movement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * movementSpeed;
    }

    public void flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void jump(float jumpForce)
    {
        _rigidBody.velocity = Vector2.up * jumpForce;

        // if (Mathf.Abs(_rigidBody.velocity.y) < 0.0001 && m_Grounded == true)
         {
        //   _rigidBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
         }
       
    }

    public void glide(float jumpForce){
        if (_rigidBody.velocity.magnitude < 2f)
        {
            _rigidBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }
}
