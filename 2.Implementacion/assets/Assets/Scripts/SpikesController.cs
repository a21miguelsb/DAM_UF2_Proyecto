using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(TakeDamage(collision.gameObject));
        }
    }
    IEnumerator TakeDamage(GameObject gameObject)
    {
        gameObject.SendMessage("TakeDamage");
        yield return new WaitForSeconds(0.5f);
    }
   
}
