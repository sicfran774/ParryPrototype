using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLockOn : MonoBehaviour
{
    public GameObject player;

    void Update()
    {
        if (player != null)
        {
            GetComponentInParent<Enemy>().LookAtPlayer(player);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            player = collision.gameObject;
            GetComponentInParent<Pathfinding.AIDestinationSetter>().target = player.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = null;
            GetComponentInParent<Pathfinding.AIDestinationSetter>().target = null;
        }
    }
}
