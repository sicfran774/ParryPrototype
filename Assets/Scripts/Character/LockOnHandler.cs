using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnHandler : MonoBehaviour
{
    public bool lockedOn = false;
    public GameObject targetedEnemy;
    public GameObject indicator;
    public SmoothCamera smoothCamera;

    [SerializeField] private List<GameObject> enemiesInRange;
    [SerializeField] private int enemyIndex;
    public GameObject player;

    void Start()
    {
        enemiesInRange = new List<GameObject>();
        indicator.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && enemiesInRange.Count > 0) //Toggle lock on
        {
            if (lockedOn) //If unlocking, don't cycle enemy
            {
                lockedOn = false;
            }
            else //If locking, choose enemy
            {
                lockedOn = true;
                CycleLockedOnEnemies();
            }
        } 
        else if (enemiesInRange.Count == 0)
        {
            lockedOn = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) && lockedOn)
        {
            CycleLockedOnEnemies();
        }

        if (lockedOn && targetedEnemy != null)
        {
            indicator.transform.position = targetedEnemy.transform.position;
            Vector3 midpoint = (player.transform.position + indicator.transform.position) / 2f;
            Vector2 offset = midpoint - player.transform.position;
            smoothCamera.offset = offset;
        }
        else
        {
            smoothCamera.offset = Vector3.zero;
        }
        indicator.SetActive(lockedOn);
    }

    void CycleLockedOnEnemies()
    {
        enemyIndex = (enemyIndex < enemiesInRange.Count - 1) ? enemyIndex + 1 : 0;
        if(enemiesInRange.Count > 0)
        {
            targetedEnemy = enemiesInRange[enemyIndex];
            indicator.transform.position = targetedEnemy.transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy") 
        {
            enemiesInRange.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "DeadEnemy")
        {
            if (enemyIndex > enemiesInRange.Count - 1 || enemiesInRange[enemyIndex] == collision.gameObject)
            {
                enemiesInRange.Remove(collision.gameObject);
                CycleLockedOnEnemies();
            }
            else
            {
                enemiesInRange.Remove(collision.gameObject);
            }
        }
    }
}
