using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Player
{
    private float turnRate = 500f;
    public GameObject aliveEyes;
    public GameObject deathEyes;

    public GameObject itemDropped;
    private bool diedAlready = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        healthBar.Initialize(health);
    }

    void Update()
    {
        healthBar.transform.position = new Vector2(transform.position.x, transform.position.y - 1.25f);

        if(!diedAlready && health <= 0)
        {
            DisableEnemy();
        }
    }

    void DisableEnemy()
    {
        diedAlready = true;
        tag = "DeadEnemy";
        healthBar.gameObject.SetActive(false);
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        GetComponent<Pathfinding.AIPath>().canMove = false;
        aliveEyes.SetActive(false);
        deathEyes.SetActive(true);
        SpawnItemDrop();
    }

    void SpawnItemDrop()
    {
        int amountDropped = Random.Range(1, 5);
        for(int i = 0; i < amountDropped; i++)
        {
            GameObject item = Instantiate(itemDropped);
            item.transform.position = transform.position;
            item.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 200);
        }
    }

    public void LookAtPlayer(GameObject player)
    {
        float x = player.transform.position.x - transform.position.x;
        float y = player.transform.position.y - transform.position.y;
        float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), turnRate * Time.deltaTime);
    }

    public IEnumerator KnockbackAndWait()
    {
        GetComponent<Pathfinding.AIPath>().canMove = false;
        yield return new WaitForSeconds(knockbackWait);
        if(!diedAlready) GetComponent<Pathfinding.AIPath>().canMove = true;
    }
}
