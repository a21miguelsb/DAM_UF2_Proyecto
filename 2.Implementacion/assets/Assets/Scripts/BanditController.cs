using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BanditController : MonoBehaviour
{

    private int lives = 3;
    private int damage = 1;
    private float timer = 0;
    private         bool droped;

    [SerializeField] float m_speed = 4.0f;


    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_Bandit m_groundSensor;
    private bool m_grounded = false;
    private bool m_combatIdle = false;
    private bool m_isDead = false;


    private SpriteRenderer m_spriteRenderer;


    private int rutina;
    private float cronometro;
    private bool atacando = false;
    private float tiempo;
    private int direccion;
    [SerializeField] float velocidad_caminar;
    [SerializeField] float velocidad_correr;

    private GameObject target;

    private float rango_vision = 5f;
    private float rango_ataque = 1f;
    private float range = 0.7f;
    [SerializeField] GameObject damage_zone;

    [SerializeField] GameObject dropObject;

    // Use this for initialization
    void Start()
    {
        target = GameObject.FindWithTag("Player");
        droped = false;
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);
        if (!m_isDead)
        {
            Comportamiento();
        }



    }

    void Comportamiento()
    {
        if (Mathf.Abs(transform.position.x - target.transform.position.x) > rango_vision && !atacando)
        {

            m_animator.SetInteger("AnimState", 1);
            cronometro += 1 * Time.deltaTime;
            if (cronometro >= tiempo)
            {
                rutina = Random.Range(0, 2);
                cronometro = 0;
            }

            switch (rutina)
            {
                case 0:
                    m_animator.SetInteger("AnimState", 1);
                    break;
                case 1:
                    direccion = Random.Range(0, 2);
                    rutina++;
                    break;
                case 2:
                    switch (direccion)
                    {
                        case 0:
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                            transform.Translate(Vector3.right * velocidad_caminar * Time.deltaTime);
                            break;
                        case 1:
                            transform.rotation = Quaternion.Euler(0, -180, 0);
                            transform.Translate(Vector3.right * velocidad_caminar * Time.deltaTime);
                            break;
                    }
                    m_animator.SetInteger("AnimState", 1);
                    break;

            }
        }
        else
        {
            if (Mathf.Abs(transform.position.x - target.transform.position.x) > rango_ataque && !atacando)
            {
                if (transform.position.x < target.transform.position.x + 1)
                {
                    m_animator.SetInteger("AnimState", 2);
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    transform.Translate(Vector3.right * velocidad_correr * Time.deltaTime);
                }
                else
                {
                    m_animator.SetInteger("AnimState", 2);
                    transform.rotation = Quaternion.Euler(0, -180, 0);
                    transform.Translate(Vector3.right * velocidad_correr * Time.deltaTime);
                }
            }
            else
            {
                if (!atacando)
                {
                    StartCoroutine(AtackAnim());

                }

            }

        }
    }

    IEnumerator AtackAnim()
    {
        if (!m_isDead)
        {
            atacando = true;
            m_animator.SetTrigger("Attack");
            yield return new WaitForSeconds(1.2f);
            m_animator.SetInteger("AnimState", 1);
            atacando = false;
        }
    }

    public void Attack()
    {
        Vector3 currentScale = damage_zone.transform.localScale;

        // Change the width by modifying the scale along the X-axis
        damage_zone.transform.localScale = new Vector3(1, currentScale.y, currentScale.z);
        damage_zone.SetActive(true);

    }
    public void EndAttack()
    {
        Vector3 currentScale = damage_zone.transform.localScale;
        // Change the width by modifying the scale along the X-axis
        damage_zone.transform.localScale = new Vector3(0, currentScale.y, currentScale.z);

        damage_zone.SetActive(false);
    }



    public void TakeDamage()
    {
        atacando = false;
        lives--;
        Vector3 position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        if (lives <= 0)
        {
            m_animator.SetTrigger("Death");
            m_isDead = true;
            timer += Time.deltaTime;


            Destroy(gameObject, 1.5f);
            

        }
        else
        {
            m_animator.SetTrigger("Hurt");
        }
        if (!droped&&m_isDead && droped==false)
            {
                dropObject.SetActive(true );
                droped=true;
            }
    }


    

    public int GetDamage()
    {
        return damage;
    }


}
