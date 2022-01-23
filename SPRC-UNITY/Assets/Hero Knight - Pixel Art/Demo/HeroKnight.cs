using UnityEngine;
using Photon.Pun;
using System;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class HeroKnight : MonoBehaviourPun
{
    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;
    [SerializeField] float m_rollForce = 6.0f;
    [SerializeField] bool m_noBlood = false;
    [SerializeField] GameObject m_slideDust;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_HeroKnight m_groundSensor;
    private bool m_grounded = false;
    private int m_facingDirection = 1;
    private int m_currentAttack = 0;
    private float m_timeSinceAttack = 0.0f;
    private float m_delayToIdle = 0.0f;

    public int maxHealth = 100;
    private int currentHealth;
    private const float SMALL_DISTANCE = 2f;
    private const float MIDDLE_DISTANCE = 3.75f;
    private const float BIG_DISTANCE = 5.5f;
    private const int smallDamageAmount = 5;
    private const int middleDamageAmount = 10;
    private const int bigDamageAmount = 20;
    private bool isAlive;

    public Transform attackPointRight;
    public Transform attackPointLeft;
    private Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayer;

    private PhotonView photonView;
    private const byte TAKE_DAMAGE_EVENT = 1;

    // Use this for initialization
    void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();

        photonView = GetComponent<PhotonView>();

        attackPoint = attackPointRight;

        isAlive = true;

        currentHealth = maxHealth;

        Hashtable customProperties = new Hashtable();
        customProperties["health"] = maxHealth;
        PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);

        Physics2D.IgnoreLayerCollision(8, 8, true);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPointRight != null)
        {
            Gizmos.DrawWireSphere(attackPointRight.position, attackRange);
        }
        else return;

        if (attackPointLeft != null)
        {
            Gizmos.DrawWireSphere(attackPointLeft.position, attackRange);
        }
        else return;
    }

    // Update is called once per frame
    void Update()
    {
        if (!base.photonView.IsMine) return;

        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        // Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);

            object[] groundedData = { "Grounded", m_grounded };
            photonView.RPC("RPC_SetBool", RpcTarget.All, groundedData);
        }

        // Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);

            object[] groundedData = { "Grounded", m_grounded };
            photonView.RPC("RPC_SetBool", RpcTarget.All, groundedData);

        }

        // Handle input and movement 
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            //this.transform.rotation = Quaternion.Euler(0, 0, 0);
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
            attackPoint = attackPointRight;

            photonView.RPC("RPC_Flip", RpcTarget.All, false);
        }

        else if (inputX < 0)
        {
            //this.transform.rotation = Quaternion.Euler(0, 180, 0);
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
            attackPoint = attackPointLeft;

            photonView.RPC("RPC_Flip", RpcTarget.All, true);
        }

        // Move
        m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        // Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        object[] airSpeedYData = { "AirSpeedY", m_body2d.velocity.y };
        photonView.RPC("RPC_SetFloat", RpcTarget.All, airSpeedYData);

        //Attack
        if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f)
        {
            m_currentAttack++;

            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack);

            photonView.RPC("RPC_SetTrigger", RpcTarget.All, "Attack" + m_currentAttack);

            // Reset timer
            m_timeSinceAttack = 0.0f;

            Attack();
        }

        //Jump
        else if (Input.GetKeyDown("space") && m_grounded)
        {
            m_animator.SetTrigger("Jump");

            photonView.RPC("RPC_SetTrigger", RpcTarget.All, "Jump");

            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);

            object[] groundedData = { "Grounded", m_grounded };
            photonView.RPC("RPC_SetBool", RpcTarget.All, groundedData);

            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);

            object[] animStateData = { "AnimState", 1 };
            photonView.RPC("RPC_SetInteger", RpcTarget.All, animStateData);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0) m_animator.SetInteger("AnimState", 0);

            object[] animStateData = { "AnimState", 0 };
            photonView.RPC("RPC_SetInteger", RpcTarget.All, animStateData);
        }
    }

    [PunRPC]
    private void RPC_SetTrigger(object data)
    {
        var animator = gameObject.GetComponent<Animator>();
        animator.SetTrigger(data.ToString());
    }

    [PunRPC]
    private void RPC_SetTriggerDeath(object data)
    {
        if (!PhotonNetwork.NickName.Equals(data.ToString())) return;

        var animator = gameObject.GetComponent<Animator>();
        animator.SetTrigger("Death");
    }

    [PunRPC]
    private void RPC_SetBool(object[] data)
    {
        var animator = gameObject.GetComponent<Animator>();

        string name = (string)data[0];
        bool value = (bool)data[1];

        animator.SetBool(name, value);
    }

    [PunRPC]
    private void RPC_SetFloat(object[] data)
    {
        var animator = gameObject.GetComponent<Animator>();

        string name = (string)data[0];
        float value = (float)data[1];

        animator.SetFloat(name, value);
    }

    [PunRPC]
    private void RPC_SetInteger(object[] data)
    {
        var animator = gameObject.GetComponent<Animator>();

        string name = (string)data[0];
        int value = (int)data[1];

        animator.SetInteger(name, value);
    }

    [PunRPC]
    private void RPC_Flip(object data)
    {
        gameObject.GetComponent<SpriteRenderer>().flipX = (bool)data;
    }

    public void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            string enemyNickName = enemy.GetComponent<PhotonView>().Owner.NickName;
            if (enemyNickName.Equals(PhotonNetwork.NickName)) continue;

            float distance = Math.Abs(this.transform.position.x - enemy.transform.position.x);
            int damage = 0;

            // Check distance
            if (distance <= SMALL_DISTANCE) damage = bigDamageAmount;
            else if (distance <= MIDDLE_DISTANCE) damage = middleDamageAmount;
            else if (distance <= BIG_DISTANCE) damage = smallDamageAmount;

            if (damage == 0) return;

            // Raise event
            object[] data = new object[] { enemyNickName, damage };
            PhotonNetwork.RaiseEvent(TAKE_DAMAGE_EVENT,
                data,
                RaiseEventOptions.Default,
                SendOptions.SendUnreliable);
        }
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    private void NetworkingClient_EventReceived(EventData obj)
    {
        // Handle event received
        if (obj.Code.Equals(TAKE_DAMAGE_EVENT))
        {
            object[] data = (object[])obj.CustomData;
            string nickName = (string)data[0];
            int damage = (int)data[1];

            if (PhotonNetwork.NickName.Equals(nickName))
            {
                TakeDamage(damage);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isAlive) return;

        currentHealth -= damage;

        if (currentHealth <= 0) currentHealth = 0;

        Hashtable customProperties = new Hashtable();
        customProperties["health"] = currentHealth;
        PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);

        m_animator.SetTrigger("Hurt");

        if (currentHealth == 0)
        {
            m_animator.SetBool("noBlood", m_noBlood);

            object[] noBloodData = { "noBlood", m_noBlood };
            photonView.RPC("RPC_SetBool", RpcTarget.All, noBloodData);

            m_animator.SetTrigger("Death");
            photonView.RPC("RPC_SetTriggerDeath", RpcTarget.AllViaServer, PhotonNetwork.NickName);

            isAlive = false;
            this.enabled = false;

            // Display game over screen
            GetComponent<PlayerScript>().GetSpawnPointScript().GameOverMenu.Show();
        }
    }
}