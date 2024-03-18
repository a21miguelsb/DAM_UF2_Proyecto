using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChestController : MonoBehaviour
{
    [SerializeField] GameObject interactMenu;
    private bool openChest = false;
    public Sprite chestOpen;
    [SerializeField] GameObject dropObject;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (openChest)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                GetComponent<SpriteRenderer>().sprite = chestOpen;
                dropObject.SetActive(true);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            interactMenu.SetActive(true);
            openChest = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            interactMenu.SetActive(false);
            openChest = false;
        }
    }
}
