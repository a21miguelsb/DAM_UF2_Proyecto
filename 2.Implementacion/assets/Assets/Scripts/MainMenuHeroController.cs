using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHeroController : MonoBehaviour
{
    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private float m_speed = 3f;
    private float m_jumpForce = 7.5f;



    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        

    }

    // Update is called once per frame
    void Update()
    {
        m_animator.SetBool("Jump",false);
        m_animator.SetInteger("AnimState", 1);
        m_animator.SetBool("Grounded", true);
        m_body2d.velocity = new Vector2(1 * m_speed, m_body2d.velocity.y);
    }

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            transform.position = new Vector3(-8.0f, transform.position.y, 0);
        }
    }


    
}
