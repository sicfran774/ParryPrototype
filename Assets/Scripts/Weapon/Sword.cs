using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Sword : MonoBehaviour
{
    public int damage;
    public float damageKnockback;
    public float swordSwingDuration;
    public float parryWindow; // Starts the moment you press "Block"

    // Range of waiting between swings
    public float enemyMinTime;
    public float enemyMaxTime;

    [SerializeField] private bool isSwinging = false;
    [SerializeField] private bool isBlocking = false;
    [SerializeField] private bool isParrying = false;
    private bool alreadyParried = false;
    private Animator animator;
    private ParticleSystem particleSys;

    [Header("Sounds")]
    private AudioSource audioSource;
    public AudioClip parrySound;

    [Header("References")]
    public EnemyLockOn enemyLockOn;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        particleSys = GetComponentInChildren<ParticleSystem>();

        particleSys.Stop();

        if (transform.parent.tag == "Enemy")
        {
            StartCoroutine(RandomTime());
        }
    }

    void Update()
    {
        if(transform.parent.tag == "Player")
        {
            PlayerAction();
        }
    }

    void PlayerAction()
    {
        if (!GetComponentInParent<PlayerController>().isStunned())
        {
            if (!isSwinging && Input.GetMouseButtonDown(0))
            {
                StartCoroutine(Swing());
            }
            else if (!isSwinging && Input.GetMouseButton(1))
            {
                isBlocking = true;
                animator.SetBool("IsBlocking", true);
            }
            else
            {
                isBlocking = false;
                animator.SetBool("IsBlocking", false);
            }

            if (isBlocking && Input.GetMouseButtonDown(1))
            {
                StartCoroutine(ParryTimer());
            }
        }
    }

    IEnumerator RandomTime()
    {
        while (true)
        {
            EnemyAttack();
            yield return new WaitForSeconds(Random.Range(enemyMinTime, enemyMaxTime));
        }
    }

    void EnemyAttack()
    {
        if (enemyLockOn.player != null && !isSwinging)
        {
            StartCoroutine(Swing());
        }
    }

    IEnumerator Swing()
    {
        isSwinging = true;
        animator.SetBool("IsSwinging", true);
        yield return new WaitForSeconds(swordSwingDuration);
        isSwinging = false;
        alreadyParried = false;
        animator.SetBool("IsSwinging", false);
        particleSys.Stop();
    }

    IEnumerator ParryTimer()
    {
        isParrying = true;
        yield return new WaitForSeconds(parryWindow);
        isParrying = false;
        particleSys.Stop();
    }

    // Remember, this is the OTHER PERSON'S sword
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isSwinging && !alreadyParried &&
           ((transform.parent.tag == "Player" && collision.tag == "Enemy") ||
           (transform.parent.tag == "Enemy" && collision.tag == "Player")))
        {
            //If the opponent's sword is parrying
            if (collision.GetComponentInChildren<Sword>().IsParrying())
            {
                alreadyParried = true;
                particleSys.Play();
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.PlayOneShot(parrySound);
            }
            else
            {
                collision.gameObject.GetComponent<Player>().TakeDamage(collision.GetComponentInChildren<Sword>().isBlocking ? Mathf.Max(1, (int)(damage * 0.5f)) : damage, transform.parent.transform.position, damageKnockback);
            }
        }
    }

    public bool IsParrying()
    {
        return isParrying;
    }

    public bool IsBlocking()
    {
        return isBlocking;
    }
}
