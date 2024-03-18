using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HeroKnight : MonoBehaviour {

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] float      m_rollForce = 6.0f;
    [SerializeField] bool       m_noBlood = false;
    [SerializeField] GameObject m_slideDust;

    [SerializeField] GameObject damage_zone;

    public int ataque;
    public InventoryController Inventory;
    [SerializeField] GameObject defend_zone;
    [SerializeField] int damage;
    [SerializeField] AudioClip walkSound;
    [SerializeField] AudioClip attakcSound;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip landSound;

    private AudioSource         m_audioSource;
    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_HeroKnight   m_groundSensor;
    private Sensor_HeroKnight   m_wallSensorR1;
    private Sensor_HeroKnight   m_wallSensorR2;
    private Sensor_HeroKnight   m_wallSensorL1;
    private Sensor_HeroKnight   m_wallSensorL2;
    private bool                m_isWallSliding = false;
    private bool                m_grounded = false;
    private bool                m_rolling = false;
    private int                 m_facingDirection = 1;
    private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;
    private float               m_delayToIdle = 0.0f;
    private float               m_rollDuration = 8.0f / 14.0f;
    private float               m_rollCurrentTime;

    private bool                isDefending = false;
    private bool                isDead = false;

    public Image barraVida;
    public float vidaActual=50;
    private float vidaMaxima = 50;
    
    
    
    private GameObject spawn;


    // Use this for initialization
    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
        m_audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update ()
    {
        barraVida.fillAmount = vidaActual / vidaMaxima;
        ataque = Inventory.equipament[0].GetComponent<ItemController>().ataque+ Inventory.equipament[1].GetComponent<ItemController>().ataque;
        spawn = GameObject.FindWithTag("Spawn");
        if (spawn != null)
        {
            transform.position = spawn.transform.position;
            Destroy(spawn);
        }
        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        // Increase timer that checks roll duration
        if(m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        if(m_rollCurrentTime > m_rollDuration)
            m_rolling = false;

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
            m_audioSource.clip = landSound;
            m_audioSource.Play();
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        if (Input.GetKey(KeyCode.D))
        {
            GetComponent<SpriteRenderer>().flipX = false;
            if (m_facingDirection == -1)
            {
                Vector3 posicionActual = damage_zone.transform.position;
                posicionActual.x = posicionActual.x + 2;
                damage_zone.transform.position = posicionActual;


                //Defend zone
                Vector3 posicionActual2 = defend_zone.transform.position;
                posicionActual2.x = posicionActual2.x + 0.6f;
                defend_zone.transform.position = posicionActual2;

            }
            m_facingDirection = 1;
            if (!m_rolling && !isDefending)
                m_body2d.velocity = new Vector2(m_facingDirection * m_speed, m_body2d.velocity.y);
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
            if (!m_audioSource.isPlaying && m_grounded)
            {
                m_audioSource.clip = walkSound;
                m_audioSource.Play();
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            GetComponent<SpriteRenderer>().flipX = true;
            if (m_facingDirection == 1)
            {
                //Damage zone
                Vector3 posicionActual = damage_zone.transform.position;
                posicionActual.x = posicionActual.x - 2;
                damage_zone.transform.position = posicionActual;

                //Defend zone
                Vector3 posicionActual2 = defend_zone.transform.position;
                posicionActual2.x = posicionActual2.x - 0.6f;
                defend_zone.transform.position = posicionActual2;

            }
            m_facingDirection = -1;
            if (!m_rolling && !isDefending)
                m_body2d.velocity = new Vector2(m_facingDirection * m_speed, m_body2d.velocity.y);
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
            if (!m_audioSource.isPlaying && m_grounded)
            {
                m_audioSource.clip = walkSound;
                m_audioSource.Play();
            }

        }

        // Move


        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- Handle Animations --
        //Wall Slide
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        m_animator.SetBool("WallSlide", m_isWallSliding);

        
    
        //Attack
        if(Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling)
        {
            defend_zone.SetActive(false);
            m_currentAttack++;

            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack);

            // Reset timer
            m_timeSinceAttack = 0.0f;

        }

        // Block
        else if (Input.GetMouseButtonDown(1) && !m_rolling)
        {
            isDefending = true;
            m_animator.SetTrigger("Block");
            defend_zone.SetActive(true);
            m_animator.SetBool("IdleBlock", true);
        }

        else if (Input.GetMouseButtonUp(1)){
            isDefending = false;
             defend_zone.SetActive(false);

            m_animator.SetBool("IdleBlock", false);
        }
                

        // Roll
        else if (Input.GetKeyDown("left shift") && !m_rolling && !m_isWallSliding)
        {
            defend_zone.SetActive(false);
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
        }
            

        //Jump
        else if (Input.GetKeyDown("space") && m_grounded && !m_rolling)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
            m_audioSource.clip = jumpSound;
            m_audioSource.Play();
        }

    

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
                if(m_delayToIdle < 0)
                    m_animator.SetInteger("AnimState", 0);
        }

        if(barraVida.fillAmount==0.0f && isDead==false)
        {
            StartCoroutine(Respawn());
        }
        
    }

    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }

    public void Attack()
    {
        Vector3 currentScale = damage_zone.transform.localScale;

        // Change the width by modifying the scale along the X-axis
        damage_zone.transform.localScale = new Vector3(1, currentScale.y, currentScale.z);
        damage_zone.transform.position = new Vector3(damage_zone.transform.position.x +0.5f, damage_zone.transform.position.y, damage_zone.transform.position.z);
        damage_zone.SetActive(true);
        m_audioSource.clip = attakcSound;
        m_audioSource.Play();

    }
    public void EndAttack()
    {
        Vector3 currentScale = damage_zone.transform.localScale;
        // Change the width by modifying the scale along the X-axis
        damage_zone.transform.localScale = new Vector3(0, currentScale.y, currentScale.z);    
                damage_zone.transform.position = new Vector3(damage_zone.transform.position.x -0.5f, damage_zone.transform.position.y, damage_zone.transform.position.z);
    
        damage_zone.SetActive(false);
    }

   

    public void TakeDamage(float damage){
        if(!isDefending && isDead==false){
            m_animator.SetTrigger("Hurt");
            vidaActual-=damage;
        }
    }

    IEnumerator Respawn()
    {
        m_animator.SetTrigger("Death");
        isDead = true;
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(1);
        vidaActual = 50;
        m_animator.SetInteger("AnimState", 0);


    }


    public int GetDamage()
    {
        return damage;
    }

   

}
