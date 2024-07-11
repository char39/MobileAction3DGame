using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    public int StartingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    Animator ani;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    bool isDead;
    bool damaged;
	void Awake ()
    {
        ani = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        currentHealth = StartingHealth;
		
	}
    private void Update()
    {
        if(damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }

    public void TakeDamage(int amount)
    {
        damaged = true;
        currentHealth -= amount;
        healthSlider.value = currentHealth;
        if(currentHealth <= 0 && !isDead )
        {
            Death();
        }
        else
        {
            ani.SetTrigger("Damage");
        }

	}
    void Death()
    {
        isDead = true;
        ani.SetTrigger("Die");
        playerMovement.enabled = false;

    }
}
