using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestArea : MonoBehaviour
{
    [Header("References")]
    public GameObject showInteract;
    public GameObject buyMenu;

    private bool playerInRange = false;
    private bool menuOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && playerInRange)
        {
            menuOpen = !menuOpen;
            buyMenu.SetActive(menuOpen);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerInRange = true;
            showInteract.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInRange = false;
            showInteract.SetActive(false);
            buyMenu.SetActive(false);
            menuOpen = false;
        }
    }
}
