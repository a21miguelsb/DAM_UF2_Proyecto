using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    [SerializeField] GameObject interactMenu;
    private bool openDoor = false;
    [SerializeField] bool forward = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (openDoor)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (forward==true)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

                }
                else
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

                }
            }
        }  
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            interactMenu.SetActive(true);
            openDoor = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            interactMenu.SetActive(false);
            openDoor = false;
        }
    }
}
