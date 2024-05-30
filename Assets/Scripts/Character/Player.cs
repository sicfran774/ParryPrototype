using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public int currency;
    public float hitIFramesSecondsCooldown;
    public float knockbackWait;
    [SerializeField] private bool isIFraming;

    protected AudioSource audioSource;
    protected Rigidbody2D rb;
    private PlayerController pc;

    [Header("References")]
    public AudioClip hurtSound;
    public HealthBar healthBar;
    public ShowDamage showDamage;
    public TMP_Text currencyText;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        pc = GetComponent<PlayerController>();
        healthBar.Initialize(health);
        maxHealth = health;
    }

    void Update()
    {
        showDamage.transform.position = transform.position;

        if (health <= 0) //Death
        {
            
        }
    }

    public void TakeDamage(int damage, Vector2 origin, float damageKnockback)
    {
        if (!isIFraming)
        {
            if (GetComponent<Enemy>() != null)
            {
                StartCoroutine(GetComponent<Enemy>().KnockbackAndWait());
            }
            else
            {
                StartCoroutine(StunnedCooldown(knockbackWait));
            }

            StartCoroutine(IFrameCooldown(hitIFramesSecondsCooldown));

            //Debug.Log(gameObject + " took " + damage + " damage");

            health -= damage;
            showDamage.Show(damage);
            healthBar.UpdateHealth(health);

            float x = transform.position.x - origin.x;
            float y = transform.position.y - origin.y;
            rb.AddForce(new Vector2(x, y) * damageKnockback);
            audioSource.PlayOneShot(hurtSound);
        }
    }

    public void Heal(int heal)
    {
        health += heal;
        currency += heal;
        currencyText.text = "" + currency;
        health = Mathf.Min(maxHealth, health);
        healthBar.UpdateHealth(health);
    }

    IEnumerator IFrameCooldown(float time)
    {
        isIFraming = true;
        yield return new WaitForSeconds(time);
        isIFraming = false;
    }

    IEnumerator StunnedCooldown(float time)
    {
        pc.setStunned(true);
        yield return new WaitForSeconds(time);
        pc.setStunned(false);
    }
}
