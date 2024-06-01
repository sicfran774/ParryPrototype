using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Sword : MonoBehaviour
{
    public int damage;
    public float damageKnockback;
    public float swingDuration;
    public float doubleSwingDuration;
    public float spinDuration;
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
            if (!isBlocking && !isSwinging && !animator.GetCurrentAnimatorStateInfo(0).IsName("Swing") && Input.GetMouseButtonDown(0))
            {
                StartCoroutine(Swing());
            }
            else if (!isSwinging && Input.GetMouseButton(1))
            {
                isBlocking = true;
            }
            else
            {
                isBlocking = false;
                animator.SetBool("IsBlocking", false);
            }
            if (Input.GetMouseButton(1))
            {
                animator.SetBool("IsBlocking", true);
            }

            if(!isSwinging && Input.GetMouseButtonDown(1))
            {
                StartCoroutine(ParryTimer());
            }
            if (alreadyParried && Input.GetMouseButtonUp(1))
            {
                alreadyParried = false;
            }
            if (isParrying)
            {
                isBlocking = true;
            }
        }
    }

    IEnumerator RandomTime()
    {
        int counter = 0;
        while (true)
        {
            EnemyAttack(counter);
            yield return new WaitForSeconds(Random.Range(enemyMinTime, enemyMaxTime));
            counter = (counter < 3) ? counter + 1 : 0;
        }
    }

    void EnemyAttack(int attack)
    {
        if (enemyLockOn.player != null && !isSwinging)
        {
            if (attack < 3)
            {
                //Debug.Log("swing");
                StartCoroutine(Swing());
                StartCoroutine(GetComponentInParent<Enemy>().StopMovingAndWait(swingDuration));
            }
            else
            {
               //Debug.Log("double swing");
                StartCoroutine(DoubleSwing());
                StartCoroutine(GetComponentInParent<Enemy>().StopMovingAndWait(doubleSwingDuration));
            }
        }
    }

    IEnumerator Swing()
    {
        
        isSwinging = true;
        animator.SetBool("IsSwinging", true);
        yield return new WaitForSeconds(swingDuration);
        isSwinging = false;
        animator.SetBool("IsSwinging", false);
    }

    IEnumerator DoubleSwing()
    {
        isSwinging = true;
        animator.SetBool("IsDoubleSwinging", true);
        yield return new WaitForSeconds(doubleSwingDuration);
        isSwinging = false;
        animator.SetBool("IsDoubleSwinging", false);
    }

    IEnumerable Spin()
    {
        isSwinging = true;
        animator.SetBool("IsSpinning", true);
        yield return new WaitForSeconds(spinDuration);
        isSwinging = false;
        animator.SetBool("IsSpinning", false);
    }

    IEnumerator ParryTimer()
    {
        isParrying = true;
        isBlocking = true;
        alreadyParried = true;
        yield return new WaitForSeconds(parryWindow);
        isParrying = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isSwinging &&
           ((transform.parent.tag == "Player" && collision.tag == "Enemy") ||
           (transform.parent.tag == "Enemy" && collision.tag == "Player")))
        {
            //Debug.Log(", origin: " + transform.parent.gameObject + ", hit: " + collision.gameObject);
            //If the opponent's sword is parrying
            if (collision.GetComponentInChildren<Sword>().IsParrying())
            {
                StartCoroutine(collision.GetComponentInChildren<Sword>().ParryParticles());
                isParrying = false;
            }
            else
            {
                collision.gameObject.GetComponent<Player>().TakeDamage(
                    collision.GetComponentInChildren<Sword>().isBlocking ?
                        Mathf.Max(1, (int)(damage * 0.5f)) : damage,
                    transform.parent.transform.position, damageKnockback
                );
            }
        }
        /*else if(isSwinging && collision.GetComponent<Sword>() != null)
        {
            
            //If the opponent's sword is parrying
            if (collision.GetComponent<Sword>().IsParrying())
            {
                StartCoroutine(collision.GetComponent<Sword>().ParryParticles());
            }
        }*/
        
    }

    public IEnumerator ParryParticles()
    {
        Debug.Log("Sound origin: " + transform.parent.gameObject);
        particleSys.Play();
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(parrySound);
        yield return new WaitForSeconds(0.2f);
        particleSys.Stop();
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
