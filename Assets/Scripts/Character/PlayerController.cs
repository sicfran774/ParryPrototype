using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed;
    public float dashSpeed;
    public float dashDecaySpeed;
    public float dashCooldown;

    [SerializeField] private float moveSpeed;
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool attacking = false;
    [SerializeField] private bool stunned = false;

    private float turnRate = 500f;

    [Header("References")]
    public GameObject lockOnIndicator;
    private Rigidbody2D rb;
    private Sword sword;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sword = transform.GetChild(0).GetComponent<Sword>();
        moveSpeed = walkSpeed;
    }
    
    void Update()
    {
        if (canDash && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(DashCooldown());
            StartCoroutine(DashDecay());
        }


        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector2 direction = new Vector2(horizontal, vertical);

        if (!stunned && !sword.IsSwinging())
        {
            rb.velocity = direction * moveSpeed;
        }

        //Rotate player
        if (lockOnIndicator.activeInHierarchy)
        {
            float x = lockOnIndicator.transform.position.x - transform.position.x;
            float y = lockOnIndicator.transform.position.y - transform.position.y;
            RotatePlayer(new Vector2(x, y));
        }
        else if (!sword.IsSwinging() && direction != Vector2.zero)
        {
            RotatePlayer(direction);
        }
    }

    void RotatePlayer(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), turnRate * Time.deltaTime);
    }

    IEnumerator DashDecay()
    {
        moveSpeed = dashSpeed;
        while(moveSpeed > walkSpeed)
        {
            moveSpeed -= dashDecaySpeed;
            yield return new WaitForSeconds(0.1f);
        }
        moveSpeed = walkSpeed;
    }

    IEnumerator DashCooldown()
    {
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public bool isStunned()
    {
        return stunned;
    }

    public void setStunned(bool _stunned)
    {
        stunned = _stunned;
    }

    public bool isAttacking()
    {
        return attacking;
    }

    public void setAttacking(bool _attacking)
    {
        attacking = _attacking;
    }
}
