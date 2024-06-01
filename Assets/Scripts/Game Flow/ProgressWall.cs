using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressWall : MonoBehaviour
{
    public bool setBeforeWalls;
    public bool setAfterWalls;

    public bool removeBeforeWallsToo;

    [SerializeField] private int enemiesInside;
    [SerializeField] private bool playerInside;

    public GameObject[] beforeWalls;
    public GameObject[] afterWalls;

    public SmoothCamera cam;
    public float zoomAmount;

    private void Start()
    {
        SetBeforeWalls(setBeforeWalls);
        SetAfterWalls(setAfterWalls);
    }

    public void SetBeforeWalls(bool condition)
    {
        foreach (GameObject wall in beforeWalls)
        {
            wall.SetActive(condition);
        }
    }

    public void SetAfterWalls(bool condition)
    {
        foreach (GameObject wall in afterWalls)
        {
            wall.SetActive(condition);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerInside = true;
            cam.ChangeZoom(zoomAmount);
        }

        if (collision.tag == "Enemy")
        {
            enemiesInside++;
        }

        if(enemiesInside > 0 && playerInside)
        {
            SetBeforeWalls(!setBeforeWalls);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInside = false;
            cam.RevertZoom();
        }

        if (collision.tag == "DeadEnemy")
        {
            enemiesInside--;
        }

        if(enemiesInside == 0)
        {
            SetBeforeWalls(false);
            SetAfterWalls(!setAfterWalls);
        }
    }
}
