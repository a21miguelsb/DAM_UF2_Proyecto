using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public List<GameObject> inventory = new List<GameObject>();
    [SerializeField] GameObject inventoryPrefab;
    [SerializeField] bool Activar_Inv;
    [SerializeField] GameObject selector;
    [SerializeField] int ID;
    private  AudioSource Audio;
    [SerializeField] AudioClip equipSound;
    [SerializeField] AudioClip selectorSound;
    [SerializeField] AudioClip deleteItemSounds;

    public List<GameObject> equipament = new List<GameObject>();
    public int IDEquipament;

    private int fasesINV;

    [SerializeField] GameObject options;
    [SerializeField] Image[] selection;
    [SerializeField] Sprite[] selections_sprites;
    [SerializeField] TextMeshProUGUI[] selections_text;
    [SerializeField] TextMeshProUGUI[] selections_text_selected;
    private int IDSelection;
    // Start is called before the first frame update
    void Start()
    {
        Audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Activar_Inv)
        {
            Navegar();
            inventoryPrefab.SetActive(true);
        }
        else{
            inventoryPrefab.SetActive(false);
        }

        if(Input.GetKeyUp(KeyCode.E)) {
            Activar_Inv = !Activar_Inv;
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].GetComponent<Image>().enabled == false) 
                {
                    inventory[i].GetComponent<Image>().enabled=true;
                    inventory[i].GetComponent<Image>().sprite = collision.GetComponent<SpriteRenderer>().sprite;
                    Destroy(collision.gameObject);
                    inventory[i].GetComponent<ItemController>().ataque = collision.GetComponent<ItemController>().ataque;
                    break;
                }
            }
        }
          
        
    }
    void Navegar()
    {

        switch (fasesINV)
        {
            case 0:
                options.SetActive(false);
                selector.SetActive(true);

                

                if (Input.GetKeyDown(KeyCode.RightArrow) && ID < inventory.Count - 1)
                {
                    ID++;
                    Audio.clip = selectorSound;
                    Audio.Play();
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow) && ID > 0)
                {
                    ID--;
                    Audio.clip = selectorSound;
                    Audio.Play();
                }
                if (Input.GetKeyDown(KeyCode.UpArrow) && ID > 3)
                {
                    ID -= 4;
                    Audio.clip = selectorSound;
                    Audio.Play();
                }
                if (Input.GetKeyDown(KeyCode.DownArrow) && ID < 4)
                {
                    ID += 4;
                    Audio.clip=selectorSound;
                    Audio.Play();
                }
                selector.transform.position = inventory[ID].transform.position;

                if (Input.GetKeyDown(KeyCode.G)&& inventory[ID].GetComponent<Image>().enabled==true)
                {
                    fasesINV = 1;
                    options.SetActive(true);

                }

                break;

            case 1:
                if ((Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.E)) )
                {
                    fasesINV = 0;
                }
                options.SetActive(true);
                options.transform.position= inventory[ID].transform.position;
                selector.SetActive(false);

                if(Input.GetKeyDown(KeyCode.UpArrow)&& IDSelection > 0)
                {
                    IDSelection--;
                }
                if (Input.GetKeyDown(KeyCode.DownArrow) && IDSelection < selection.Length - 1)
                {
                    IDSelection++;
                }

                switch (IDSelection)
                {
                    case 0:
                        selection[0].sprite = selections_sprites[1];
                        selections_text[1].gameObject.SetActive(true);
                        selections_text_selected[1].gameObject.SetActive(false);
                        selections_text[0].gameObject.SetActive(false);
                        selections_text_selected[0].gameObject.SetActive(true);
                        selection[1].sprite = selections_sprites[0];

                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            
                            if (equipament[IDEquipament].GetComponent<Image>().enabled == false)
                            {
                                

                                equipament[IDEquipament].GetComponent<Image>().sprite = inventory[ID].GetComponent<Image>().sprite;
                                equipament[IDEquipament].GetComponent<Image>().enabled = true;
                                inventory[ID].GetComponent<Image>().sprite=null;
                                inventory[ID].GetComponent<Image>().enabled=false;
                                equipament[IDEquipament].GetComponent<ItemController>().ataque = inventory[ID].GetComponent<ItemController>().ataque;
                                if (IDEquipament == 0)
                                {
                                    IDEquipament++;
                                }
                                else
                                {
                                    IDEquipament--;
                                }
                            }
                            else
                            {

                                Sprite obj = inventory[ID].GetComponent<Image>().sprite;
                                int Ataque = inventory[ID].GetComponent<ItemController>().ataque;

                                inventory[ID].GetComponent <Image>().sprite = equipament[IDEquipament].GetComponent<Image>().sprite;
                                equipament[IDEquipament].GetComponent <Image>().sprite=obj;

                                inventory[ID].GetComponent<ItemController>().ataque = equipament[IDEquipament].GetComponent<ItemController>().ataque;
                                equipament[IDEquipament].GetComponent<ItemController>().ataque = Ataque;
                            
                            }


                            fasesINV = 0;
                        }
                        break;
                    case 1:
                        selection[0].sprite = selections_sprites[0];
                        selection[1].sprite = selections_sprites[1];
                        selections_text[1].gameObject.SetActive(false);
                        selections_text_selected[1].gameObject.SetActive(true);

                        selections_text[0].gameObject.SetActive(true);
                        selections_text_selected[0].gameObject.SetActive(false);


                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            inventory[ID].GetComponent<Image>().sprite = null;
                            inventory[ID].GetComponent<Image>().enabled=false;
                            options.SetActive(false);
                            inventory[ID].GetComponent<ItemController>().ataque = 0;
                            fasesINV = 0;
                            Audio.clip = deleteItemSounds;
                            Audio.Play();
                        }

                        break;
                }

                break;
        }
        
    }
}
