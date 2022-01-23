using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float m_speed = 4.0f;
    public float m_jumpForce = 7.5f;
    public float m_rollForce = 6.0f;

    bool touchedGround = false;
    private int m_facingDirection = 1;
    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private bool m_rolling = false;
    private int m_currentAttack = 0;
    private float m_delayToIdle = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {   
        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)//right
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }

        else if (inputX < 0)//left
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }

        // Move
        if (!m_rolling)
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Death
        if (Input.GetKeyDown("e") && !m_rolling)
        {
            m_animator.SetTrigger("Death");
        }

        //Hurt
        else if (Input.GetKeyDown("q") && !m_rolling)
        {
            m_animator.SetTrigger("Hurt");
        }

        //Attack
        else if (Input.GetMouseButtonDown(0) && !m_rolling)
        {
            m_currentAttack++;

            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack);
        }

        // Block
        else if (Input.GetMouseButtonDown(1) && !m_rolling)
        {
            m_animator.SetTrigger("Block");
            m_animator.SetBool("IdleBlock", true);
        }

        else if (Input.GetMouseButtonUp(1))
            m_animator.SetBool("IdleBlock", false);

        // Roll
        else if (Input.GetKeyDown("left shift") && !m_rolling)
        {
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
        }

        //Jump
        else if (Input.GetKeyDown("space") && touchedGround && !m_rolling)
        {
            m_animator.SetTrigger("Jump");
            touchedGround = false;
            m_animator.SetBool("Grounded", touchedGround);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            //m_groundSensor.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                m_animator.SetInteger("AnimState", 0);
        }
    }

    // Animation Events
    // Called in end of roll animation.
    void StopRolling()
    {
        m_rolling = false;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("coliziune");
        //Check to see if the tag on the collider is equal to Ground
        if (col.collider.tag == "Ground")
        {
            touchedGround = true;
            Debug.Log("grounded= "+ touchedGround);
        }
    }
}
